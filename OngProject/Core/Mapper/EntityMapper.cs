using AutoMapper;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Mapper
{
    public class EntityMapper: Profile
    {
        public EntityMapper()
        {
            CreateMap<Members, MemberDTO>();
        }
    }
}
