using FluentValidation;
using PhotoAlbum.Backend.Common.Dtos.Account;
using System;
using System.Collections.Generic;

namespace PhotoAlbum.Backend.Common.Dtos.Image
{
    public class ImageEditDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
    }
}
