using PhotoAlbum.Backend.Common.Dtos.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoAlbum.Backend.Common.Dtos.Album;

namespace PhotoAlbum.Backend.Bll.Services.Image
{
    public interface IImageService
    {
        Task<ImageDto> UploadImageAsync(int albumId, IFormFile file);
        Task DeleteImageAsync(int imageId);
        Task<CommentDto> PostCommentAsync(int imageId, string text);
        Task DeleteCommentAsync(int commentId);
        Task<ImageDto> EditImageAsync(ImageEditDto imageEditDto);
    }
}
