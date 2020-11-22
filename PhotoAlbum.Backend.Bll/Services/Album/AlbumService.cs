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
using PhotoAlbum.Backend.Common.Dtos.Album;
using PhotoAlbum.Backend.Common.Dtos.Account;
using System.IO.Compression;

namespace PhotoAlbum.Backend.Bll.Services.Image
{
    public class AlbumService : IAlbumService
    {
        private readonly ImageOptions _imageOptions;
        private readonly ClaimsPrincipal _user;
        private readonly UserManager<User> _userManager;
        private readonly PhotoAlbumDbContext _dbContext;

        public AlbumService(IOptions<ImageOptions> imageOptions, UserManager<User> userManager, ClaimsPrincipal user, PhotoAlbumDbContext dbContext)
        {
            _imageOptions = imageOptions.Value;
            _userManager = userManager;
            _user = user;
            _dbContext = dbContext;
        }

        public async Task<List<AlbumDto>> GetAlbumsAsync()
        {
            var user = await _userManager.GetUserAsync(_user);

            return await _dbContext.Albums.Where(a => a.Creator == user).Select(a => new AlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                Path = a.Path,
                Creator = new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email },
                Images = a.Images.Select(i => new ImageDto { 
                    Id = i.Id, 
                    FileName = i.FileName, 
                    Path = a.Path + "/" + i.FileName, 
                    Date = i.Date, 
                    Location = i.Location, 
                    Tags = i.Tags.Select(t => t.Text).ToList(), 
                    Comments = i.Comments.Select(c => new CommentDto { Commenter = new UserDto { UserName = c.Commenter.UserName, Email = c.Commenter.Email, Id = c.Commenter.Id }, Date = c.Date, Id = c.Id, Text = c.Text }).ToList(),
                    Uploader = new UserDto { Id = a.Creator.Id, UserName = a.Creator.UserName, Email = a.Creator.Email}
                }).ToList()
            }).ToListAsync();
        }

        public async Task<List<AlbumDto>> GetSharedAlbumsAsync()
        {
            var albums = new List<AlbumDto>();

            var usr = await _userManager.GetUserAsync(_user);

            var user = await _dbContext.Users.Include(u => u.Albums).Include(u => u.Groups).FirstOrDefaultAsync(u => u.Id == usr.Id);

            foreach (var a in user.Albums)
            {
                var album = await _dbContext.Albums
                    .Include(al => al.Creator)
                    .Include(al => al.Images).ThenInclude(i => i.Tags)
                    .Include(al => al.Images).ThenInclude(i => i.Comments).ThenInclude(c => c.Commenter)
                    .FirstOrDefaultAsync(al => al.Id == a.AlbumId);

                albums.Add(new AlbumDto
                {
                    Id = a.AlbumId,
                    Name = album.Name,
                    Path = album.Path,
                    Creator = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email },
                    Images = album.Images.Select(i => new ImageDto
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        Path = album.Path + "/" + i.FileName,
                        Date = i.Date,
                        Location = i.Location,
                        Tags = i.Tags.Select(t => t.Text).ToList(),
                        Comments = i.Comments.Select(c => new CommentDto { Commenter = new UserDto { UserName = c.Commenter.UserName, Email = c.Commenter.Email, Id = c.Commenter.Id }, Date = c.Date, Id = c.Id, Text = c.Text }).ToList(),
                        Uploader = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email }
                    }).ToList()
                });
            }

            foreach (var g in user.Groups)
            {
                foreach (var a in g.Group.Albums)
                {
                    if (!albums.Any(album => album.Id == a.AlbumId))
                    {
                        var album = await _dbContext.Albums
                            .Include(al => al.Creator)
                            .Include(al => al.Images).ThenInclude(i => i.Tags)
                            .Include(al => al.Images).ThenInclude(i => i.Comments).ThenInclude(c => c.Commenter)
                            .FirstOrDefaultAsync(al => al.Id == a.AlbumId);

                        albums.Add(new AlbumDto
                        {
                            Id = a.AlbumId,
                            Name = album.Name,
                            Path = album.Path,
                            Creator = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email },
                            Images = album.Images.Select(i => new ImageDto
                            {
                                Id = i.Id,
                                FileName = i.FileName,
                                Path = album.Path + "/" + i.FileName,
                                Date = i.Date,
                                Location = i.Location,
                                Tags = i.Tags.Select(t => t.Text).ToList(),
                                Comments = i.Comments.Select(c => new CommentDto { Commenter = new UserDto { UserName = c.Commenter.UserName, Email = c.Commenter.Email, Id = c.Commenter.Id }, Date = c.Date, Id = c.Id, Text = c.Text }).ToList(),
                                Uploader = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email }
                            }).ToList()
                        });
                    }
                }
            }

            return albums;
        }

        public async Task<AlbumDto> GetPublicAlbumAsync(string albumLink)
        {
            var album = await _dbContext.Albums
                .Include(al => al.Creator)
                .Include(al => al.Images).ThenInclude(i => i.Tags)
                .Include(al => al.Images).ThenInclude(i => i.Comments).ThenInclude(c => c.Commenter)
                .FirstOrDefaultAsync(a => a.Path.Equals(albumLink));

            if (album == null)
                throw new PhotoAlbumException($"The album does not exist", 404);

            return new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                Path = album.Path,
                Creator = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email },
                Images = album.Images.Select(i => new ImageDto
                {
                    Id = i.Id,
                    FileName = i.FileName,
                    Path = album.Path + "/" + i.FileName,
                    Date = i.Date,
                    Location = i.Location,
                    Tags = i.Tags.Select(t => t.Text).ToList(),
                    Comments = i.Comments.Select(c => new CommentDto { Commenter = new UserDto { UserName = c.Commenter.UserName, Email = c.Commenter.Email, Id = c.Commenter.Id }, Date = c.Date, Id = c.Id, Text = c.Text }).ToList(),
                    Uploader = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email }
                }).ToList()
            };
        }

        public async Task<AlbumDto> CreateAlbumAsync(string albumName)
        {
            var albumPath = Guid.NewGuid().ToString();

            var album = new Album
            {
                Creator = await _userManager.GetUserAsync(_user),
                Name = albumName,
                Path = albumPath
            };

            _dbContext.Albums.Add(album);
            await _dbContext.SaveChangesAsync();

            return new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                Creator = new UserDto { Id = album.Creator.Id, UserName = album.Creator.UserName, Email = album.Creator.Email },
                Images = new List<ImageDto>(),
                Path = album.Path
            };
        }

        public async Task RenameAlbumAsync(int albumId, string albumName)
        {
            var user = await _userManager.GetUserAsync(_user);

            var album = await _dbContext.Albums.FindAsync(albumId);

            if (album == null)
                throw new PhotoAlbumException($"Album with id '{albumId}' does not exist", 404);

            if (album.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to rename album with id '{albumId}'", 401);

            album.Name = albumName;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAlbumAsync(int albumId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var album = await _dbContext.Albums.FindAsync(albumId);

            if (album == null) 
                throw new PhotoAlbumException($"Album with id '{albumId}' does not exist", 404);

            if (album.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to delete album with id '{albumId}'", 401);

            var albumPath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, album.Path);

            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync();

            if (Directory.Exists(albumPath))
                Directory.Delete(albumPath, true);
        }

        public async Task ShareAlbumWithGroup(int albumId, int groupId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var album = await _dbContext.Albums.FindAsync(albumId);

            if (album == null)
                throw new PhotoAlbumException($"Album with id '{albumId}' does not exist", 404);

            if (album.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to share album with id '{albumId}'", 401);

            var group = await _dbContext.Groups.Include(g => g.Albums).FirstOrDefaultAsync(g=> g.Id == groupId);

            if (group == null)
                throw new PhotoAlbumException($"Group with id '{groupId}' does not exist", 404);

            if (!user.CreatedGroups.Contains(group))
                throw new PhotoAlbumException($"You do not have authorization to share to group with id '{groupId}'", 401);

            group.Albums.Add(new AlbumGroup { AlbumId = albumId, GroupId = groupId });
            await _dbContext.SaveChangesAsync();
        }

        public async Task ShareAlbumWithUser(int albumId, int userId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var album = await _dbContext.Albums.FindAsync(albumId);

            if (album == null)
                throw new PhotoAlbumException($"Album with id '{albumId}' does not exist", 404);

            if (album.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to share album with id '{albumId}'", 401);

            var shareUser = await _dbContext.Users.Include(u => u.Albums).FirstOrDefaultAsync(u => u.Id == userId);

            if (shareUser == null)
                throw new PhotoAlbumException($"User with id '{userId}' does not exist", 404);

            shareUser.Albums.Add(new AlbumUser { AlbumId = albumId, UserId = userId });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> DownloadAlbumAsync(int albumId)
        {
            var album = await _dbContext.Albums.FindAsync(albumId);
            if (album == null)
                throw new PhotoAlbumException($"Album with id '{albumId}' does not exist", 404);

            var albumPath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, album.Path);
            if (!Directory.Exists(albumPath) || Directory.GetFiles(albumPath).Length == 0)
                throw new PhotoAlbumException($"Album with id '{albumId}' does not contain any images", 404);

            var zipsPath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, ".zips");
            if (!Directory.Exists(zipsPath))
                Directory.CreateDirectory(zipsPath);

            var zipName = Guid.NewGuid().ToString() + ".zip";
            var zipPath = Path.Combine(zipsPath, zipName);

            ZipFile.CreateFromDirectory(albumPath, zipPath);

            if (File.Exists(zipPath))
            {
                return zipName;
            }
            else throw new PhotoAlbumException($"Error while compressing album with id '{albumId}'", 404);
        }
    }
}
