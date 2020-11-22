using PhotoAlbum.Backend.Common.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Common.Dtos.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
