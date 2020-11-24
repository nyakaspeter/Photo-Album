using PhotoAlbum.Backend.Bll.Services.Image;
using PhotoAlbum.Backend.Common.Constants;
using PhotoAlbum.Backend.Common.Dtos.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoAlbum.Backend.Common.Dtos.Album;
using System;

namespace PhotoAlbum.Backend.Web.Controllers
{
    [Authorize]
    public class AlbumController : ApiControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<List<AlbumDto>> GetAlbums()
        {
            return await _albumService.GetAlbumsAsync();
        }

        [HttpPost]
        public async Task<AlbumDto> CreateAlbum(string albumName)
        {
            return await _albumService.CreateAlbumAsync(albumName);
        }

        [HttpPut]
        public async Task RenameAlbum(int albumId, string albumName)
        {
            await _albumService.RenameAlbumAsync(albumId, albumName);
        }

        [HttpDelete]
        public async Task DeleteAlbum(int albumId)
        {
            await _albumService.DeleteAlbumAsync(albumId);
        }

        [HttpGet("shared")]
        public async Task<List<AlbumDto>> GetSharedAlbums()
        {
            return await _albumService.GetSharedAlbumsAsync();
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<AlbumDto> GetPublicAlbum(string albumLink)
        {
            return await _albumService.GetPublicAlbumAsync(albumLink);
        }

        [AllowAnonymous]
        [HttpGet("download")]
        public async Task<string> DownloadAlbum(int albumId)
        {
            return await _albumService.DownloadAlbumAsync(albumId);
        }

        [HttpPost("usershare")]
        public async Task ShareAlbumWithUser(int albumId, int userId)
        {
            await _albumService.ShareAlbumWithUser(albumId, userId);
        }

        [HttpDelete("usershare")]
        public async Task UnshareAlbumWithUser(int albumId, int userId)
        {
            await _albumService.UnshareAlbumWithUser(albumId, userId);
        }

        [HttpPost("groupshare")]
        public async Task ShareAlbumWithGroup(int albumId, int groupId)
        {
            await _albumService.ShareAlbumWithGroup(albumId, groupId);
        }

        [HttpDelete("groupshare")]
        public async Task UnshareAlbumWithGroup(int albumId, int groupId)
        {
            await _albumService.UnshareAlbumWithGroup(albumId, groupId);
        }
    }
}
