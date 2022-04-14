using AutoMapper;
using BIFastWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RespAliasManagement, AliasManagementResponses>().ReverseMap();
            CreateMap<RespErrAliasManagement, AliasManagementResponses>().ReverseMap();
            CreateMap<RespRejectAliasManagement, AliasManagementResponses>().ReverseMap();
        }
    }
}