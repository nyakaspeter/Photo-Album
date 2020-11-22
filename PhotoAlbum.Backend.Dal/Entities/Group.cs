using System.Collections.Generic;

namespace PhotoAlbum.Backend.Dal.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupUser> Users { get; set; }
        public List<AlbumGroup> Albums { get; set; }
        public User Creator { get; set; }
    }
}