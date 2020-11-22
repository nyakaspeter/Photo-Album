using AutoMapper;
using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Common.Dtos.Image;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Mapper
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDto>();
        }
    }
}
