using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Entities
{
    public class User : IdentityUser<int>
    {
        public List<Image> Images { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AlbumUser> Albums { get; set; }
        public List<Album> CreatedAlbums { get; set; }
        public List<GroupUser> Groups { get; set; }
        public List<Group> CreatedGroups { get; set; }
    }
}
