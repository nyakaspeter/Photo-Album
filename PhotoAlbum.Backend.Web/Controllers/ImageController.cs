using PhotoAlbum.Backend.Bll.Services.Image;
using PhotoAlbum.Backend.Common.Constants;
using PhotoAlbum.Backend.Common.Dtos.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace PhotoAlbum.Backend.Web.Controllers
{
    [Authorize]
    public class ImageController : ApiControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ImageDto> UploadImage(int albumId, IFormFile file)
        {
            return await _imageService.UploadImageAsync(albumId, file);
        }

        [HttpDelete]
        public async Task DeleteImage(int imageId)
        {
            await _imageService.DeleteImageAsync(imageId);
        }

        [HttpPost("comment")]
        public async Task<CommentDto> PostComment(int imageId, string text)
        {
            return await _imageService.PostCommentAsync(imageId, text);
        }

        [HttpDelete("comment")]
        public async Task DeleteComment(int commentId)
        {
            await _imageService.DeleteCommentAsync(commentId);
        }
    }
}
