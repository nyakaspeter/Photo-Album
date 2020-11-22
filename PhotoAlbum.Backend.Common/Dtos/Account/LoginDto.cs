using FluentValidation;

namespace PhotoAlbum.Backend.Common.Dtos.Account
{
    public class LoginDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginUserDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
