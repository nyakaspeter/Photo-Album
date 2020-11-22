using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Entities
{
    public class AlbumGroup
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
