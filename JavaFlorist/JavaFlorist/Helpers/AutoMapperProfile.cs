using System;
using AutoMapper;
using JavaFlorist.Models;
using JavaFlorist.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JavaFlorist.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserViewModel, UserModel>();

        }
    }
}

