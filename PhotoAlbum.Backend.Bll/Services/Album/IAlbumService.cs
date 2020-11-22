using PhotoAlbum.Backend.Common.Dtos.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoAlbum.Backend.Common.Dtos.Album;

namespace PhotoAlbum.Backend.Bll.Services.Image
{
    public interface IAlbumService
    {
        Task<List<AlbumDto>> GetSharedAlbumsAsync();
        Task<AlbumDto> CreateAlbumAsync(string albumName);
        Task RenameAlbumAsync(int albumId, string albumName);
        Task DeleteAlbumAsync(int albumId);
        Task<AlbumDto> GetPublicAlbumAsync(string albumLink);
        Task<string> DownloadAlbumAsync(int albumId);
        Task ShareAlbumWithUser(int albumId, int userId);
        Task ShareAlbumWithGroup(int albumId, int groupId);
        Task<List<AlbumDto>> GetAlbumsAsync();
    }
}
