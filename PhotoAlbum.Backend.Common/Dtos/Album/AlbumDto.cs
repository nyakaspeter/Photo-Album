using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Common.Dtos.Group;
using PhotoAlbum.Backend.Common.Dtos.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Common.Dtos.Album
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public UserDto Creator { get; set; }
        public List<ImageDto> Images { get; set; }
        public List<UserDto> UsersWithAccess { get; set; }
        public List<GroupDto> GroupsWithAccess { get; set; }
    }
}
