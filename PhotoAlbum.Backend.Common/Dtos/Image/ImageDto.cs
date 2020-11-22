using FluentValidation;
using PhotoAlbum.Backend.Common.Dtos.Account;
using System;
using System.Collections.Generic;

namespace PhotoAlbum.Backend.Common.Dtos.Image
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public UserDto Uploader { get; set; }
        public List<string> Tags { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
