using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public User Creator { get; set; }
        public List<Image> Images { get; set; }
        public List<AlbumUser> Users { get; set; }
        public List<AlbumGroup> Groups { get; set; }
    }
}
