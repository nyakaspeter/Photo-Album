using PhotoAlbum.Backend.Bll.Services.Account;
using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhotoAlbum.Backend.Common.Dtos.Group;
using System.Collections.Generic;
using System;

namespace PhotoAlbum.Backend.Web.Controllers
{
    [Authorize]
    public class GroupController : ApiControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<List<GroupDto>> GetGroups()
        {
            return await _groupService.GetGroupsAsync();
        }

        [HttpPost]
        public async Task<GroupDto> CreateGroup(string groupName)
        {
            return await _groupService.CreateGroupAsync(groupName);
        }

        [HttpPut]
        public async Task RenameGroup(int groupId, string groupName)
        {
            await _groupService.RenameGroupAsync(groupId, groupName);
        }

        [HttpDelete]
        public async Task DeleteGroup(int groupId)
        {
            await _groupService.DeleteGroupAsync(groupId);
        }

        [HttpPost("user")]
        public async Task AddUserToGroup(int userId, int groupId)
        {
            await _groupService.AddUserToGroupAsync(userId, groupId);
        }

        [HttpDelete("user")]
        public async Task DeleteUserFromGroup(int userId, int groupId)
        {
            await _groupService.DeleteUserFromGroupAsync(userId, groupId);
        }
    }
}
