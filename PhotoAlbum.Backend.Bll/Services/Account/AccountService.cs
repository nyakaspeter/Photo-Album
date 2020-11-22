using AutoMapper;
using PhotoAlbum.Backend.Common.Constants;
using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Common.Exceptions;
using PhotoAlbum.Backend.Dal;
using PhotoAlbum.Backend.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PhotoAlbum.Backend.Bll.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PhotoAlbumDbContext _dbContext;
        
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<User> signInManager, PhotoAlbumDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _dbContext.Users.Select(u => new UserDto { Id = u.Id, UserName = u.UserName, Email = u.Email }).ToListAsync();
        }

        public async Task LoginAsync(LoginDto dto)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            if (!signInResult.Succeeded)
                throw new PhotoAlbumException("Wrong username or password");
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            User userEntity = _mapper.Map<User>(dto);

            if (await _dbContext.Users.AnyAsync(u => u.NormalizedUserName == dto.UserName.ToUpper()))
                throw new PhotoAlbumException("The selected username is taken");

            var createdUser = await _userManager.CreateAsync(userEntity, dto.Password);
            if (!createdUser.Succeeded)
                throw new PhotoAlbumException("Error during registration");

            await _userManager.AddToRoleAsync(userEntity, Roles.User);
        }
    }
}
