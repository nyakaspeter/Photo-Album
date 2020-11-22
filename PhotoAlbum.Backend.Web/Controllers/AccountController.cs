using PhotoAlbum.Backend.Bll.Services.Account;
using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace PhotoAlbum.Backend.Web.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly JwtHelper _jwtHelper;

        public AccountController(IAccountService accountService, JwtHelper jwtHelper)
        {
            _accountService = accountService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<TokenDto> Login(LoginDto userDto)
        {
            await _accountService.LoginAsync(userDto);
            return new TokenDto() { Token = await _jwtHelper.GenerateJsonWebToken(userDto) };
        }

        [HttpPost("register")]
        public async Task Register(RegisterDto registerDto)
        {
            await _accountService.RegisterAsync(registerDto);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<List<UserDto>> GetUsers()
        {
            return await _accountService.GetUsersAsync();
        }
    }
}
