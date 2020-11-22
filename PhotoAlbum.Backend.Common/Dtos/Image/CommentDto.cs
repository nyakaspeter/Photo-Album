using FluentValidation;
using PhotoAlbum.Backend.Common.Dtos.Account;
using System;

namespace PhotoAlbum.Backend.Common.Dtos.Image
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public UserDto Commenter { get; set; }
    }
}
