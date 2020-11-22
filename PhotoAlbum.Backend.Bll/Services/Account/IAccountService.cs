using PhotoAlbum.Backend.Common.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Backend.Bll.Services.Account
{
    public interface IAccountService
    {
        Task LoginAsync(LoginDto dto);
        Task RegisterAsync(RegisterDto registerDto);
        Task<List<UserDto>> GetUsersAsync();
    }
}
