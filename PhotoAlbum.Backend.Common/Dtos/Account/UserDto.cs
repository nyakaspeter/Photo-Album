using FluentValidation;

namespace PhotoAlbum.Backend.Common.Dtos.Account
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
