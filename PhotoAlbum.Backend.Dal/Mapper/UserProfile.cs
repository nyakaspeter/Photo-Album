using AutoMapper;
using PhotoAlbum.Backend.Common.Dtos.Account;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginDto, User>();
            CreateMap<RegisterDto, User>();
        }
    }
}
