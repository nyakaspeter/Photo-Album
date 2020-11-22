using PhotoAlbum.Backend.Common.Dtos.Image;
using PhotoAlbum.Backend.Common.Exceptions;
using PhotoAlbum.Backend.Common.Options;
using PhotoAlbum.Backend.Dal;
using PhotoAlbum.Backend.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PhotoAlbum.Backend.Common.Dtos.Account;

namespace PhotoAlbum.Backend.Bll.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly ImageOptions _imageOptions;
        private readonly ClaimsPrincipal _user;
        private readonly UserManager<User> _userManager;
        private readonly PhotoAlbumDbContext _dbContext;

        public ImageService(IOptions<ImageOptions> fileOptions, UserManager<User> userManager, ClaimsPrincipal user, PhotoAlbumDbContext dbContext)
        {
            _imageOptions = fileOptions.Value;
            _userManager = userManager;
            _user = user;
            _dbContext = dbContext;
        }

        public async Task<ImageDto> UploadImageAsync(int albumId, IFormFile file)
        {
            var user = await _userManager.GetUserAsync(_user);

            var album = await _dbContext.Albums.FindAsync(albumId);

            if (album == null)
                throw new PhotoAlbumException($"Album with id '{albumId}' does not exist", 404);

            if (album.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to modify album with id '{albumId}'", 401);

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !_imageOptions.AllowedFileExtensions.Contains(extension))
                throw new PhotoAlbumException($"'{extension}' extension is not allowed");

            var albumPath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, album.Path);
            if (!Directory.Exists(albumPath))
                Directory.CreateDirectory(albumPath);

            var imagePath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, album.Path, file.FileName);
            if (File.Exists(imagePath))
                throw new PhotoAlbumException($"A file with the same name already exists in the album with id '{albumId}'", 400);

            var image = new Dal.Entities.Image
            {
                Album = album,
                Date = DateTime.UtcNow,
                Location = "Unknown",
                FileName = file.FileName,
                Uploader = await _userManager.GetUserAsync(_user)
            };

            _dbContext.Images.Add(image);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _dbContext.SaveChangesAsync();

            return new ImageDto
            {
                Id = image.Id,
                FileName = image.FileName,
                Path = album.Path + "/" + image.FileName,
                Date = image.Date,
                Location = image.Location,
                Tags = new List<string>()
            };
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var image = await _dbContext.Images.Include(i => i.Album).FirstOrDefaultAsync(i => i.Id == imageId);
            
            if (image is null)
                throw new PhotoAlbumException($"Image with id '{imageId}' does not exist", 404);

            if (image.Uploader != user)
                throw new PhotoAlbumException($"You do not have authorization to delete image with id '{imageId}'", 401);

            var imagePath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, image.Album.Path, image.FileName);

            if (File.Exists(imagePath))
                File.Delete(imagePath);

            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommentDto> PostCommentAsync(int imageId, string text)
        {
            var user = await _userManager.GetUserAsync(_user);

            var image = await _dbContext.Images.FindAsync(imageId);
            if (image is null)
                throw new PhotoAlbumException($"Image with id '{imageId}' does not exist");

            var comment = new Comment
            {
                Image = image,
                Text = text,
                Commenter = await _userManager.GetUserAsync(_user),
                Date = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            return new CommentDto
            {
                Id = comment.Id,
                Commenter = new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email },
                Text = comment.Text,
                Date = comment.Date
            };
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment is null)
                throw new PhotoAlbumException($"Comment with id '{commentId}' does not exist");

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
