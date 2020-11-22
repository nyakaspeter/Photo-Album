﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhotoAlbum.Backend.Common.Options
{
    public class ImageOptions
    {
        public string RootPath { get; set; } = Directory.GetCurrentDirectory();
        public string FilesPath { get; set; }
        public string[] AllowedFileExtensions { get; set; }
    }
}
