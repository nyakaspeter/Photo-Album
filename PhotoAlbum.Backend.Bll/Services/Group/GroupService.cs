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
using PhotoAlbum.Backend.Common.Dtos.Group;
using System.Collections.Generic;
using System.Linq;

namespace PhotoAlbum.Backend.Bll.Services.Account
{
    public class GroupService : IGroupService
    {
        private readonly UserManager<User> _userManager;
        private readonly ClaimsPrincipal _user;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PhotoAlbumDbContext _dbContext;
        
        private readonly IMapper _mapper;

        public GroupService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, ClaimsPrincipal user, SignInManager<User> signInManager, PhotoAlbumDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _user = user;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<GroupDto>> GetGroupsAsync()
        {
            var user = await _userManager.GetUserAsync(_user);

            return await _dbContext.Groups.Where(g => g.Creator == user).Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Users = g.Users.Select(u => new UserDto { Id = u.UserId, UserName = u.User.UserName, Email = u.User.Email }).ToList()
            }).ToListAsync();
        }

        public async Task<GroupDto> CreateGroupAsync(string groupName)
        {
            var user = await _userManager.GetUserAsync(_user);

            var group = new Group
            {
                Name = groupName,
                Creator = user
            };

            _dbContext.Groups.Add(group);
            await _dbContext.SaveChangesAsync();

            return new GroupDto { Id = group.Id, Name = group.Name, Users = new List<UserDto>() };
        }

        public async Task RenameGroupAsync(int groupId, string groupName)
        {
            var user = await _userManager.GetUserAsync(_user);

            var group = await _dbContext.Groups.FindAsync(groupId);

            if (group == null)
                throw new PhotoAlbumException($"Group with id '{groupId}' does not exist", 404);

            if (group.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to rename group with id '{groupId}'", 401);

            group.Name = groupName;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var group = await _dbContext.Groups.FindAsync(groupId);

            if (group == null)
                throw new PhotoAlbumException($"Group with id '{groupId}' does not exist", 404);

            if (group.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to delete group with id '{groupId}'", 401);

            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserToGroupAsync(int userId, int groupId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var group = await _dbContext.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                throw new PhotoAlbumException($"Group with id '{groupId}' does not exist", 404);

            if (group.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to modify group with id '{groupId}'", 401);

            group.Users.Add(new GroupUser { GroupId = groupId, UserId = userId });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserFromGroupAsync(int userId, int groupId)
        {
            var user = await _userManager.GetUserAsync(_user);

            var group = await _dbContext.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
                throw new PhotoAlbumException($"Group with id '{groupId}' does not exist", 404);

            if (group.Creator != user)
                throw new PhotoAlbumException($"You do not have authorization to modify group with id '{groupId}'", 401);

            var groupUser = group.Users.FirstOrDefault(gu => gu.UserId == userId);

            if (groupUser == null)
                throw new PhotoAlbumException($"User with id '{userId}' is not in the group with id '{groupId}'", 404);

            group.Users.Remove(groupUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}
