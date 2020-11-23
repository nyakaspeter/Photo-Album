using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public Album Album { get; set; }
        public string Tags { get; set; }
        public User Uploader { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
