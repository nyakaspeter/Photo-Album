﻿namespace PhotoAlbum.Backend.Dal.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }
    }
}