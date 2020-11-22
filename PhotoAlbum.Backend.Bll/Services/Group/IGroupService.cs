using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Common.Dtos.Group;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Backend.Bll.Services.Account
{
    public interface IGroupService
    {
        Task<List<GroupDto>> GetGroupsAsync();
        Task<GroupDto> CreateGroupAsync(string groupName);
        Task RenameGroupAsync(int groupId, string groupName);
        Task DeleteGroupAsync(int groupId);
        Task AddUserToGroupAsync(int userId, int groupId);
        Task DeleteUserFromGroupAsync(int userId, int groupId);
    }
}
