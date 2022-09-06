using AutoMapper;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.Category;
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
            CreateMap<MemberDTO, Members>();
            CreateMap<EditMemberDTO, Members>();
            CreateMap<Members, EditMemberDTO>();

            // Categories
            CreateMap<Categories, CategoryDTO>();
            CreateMap<CategoryDTO, Categories>();
            CreateMap<CategoryEditDTO, Categories>();
            CreateMap<Categories, CategoryEditDTO>();
            CreateMap<Categories, CategoryGetAllNamesResponse>();
            CreateMap<CategoryGetAllNamesResponse, Categories>();

            // Roles
            CreateMap<Roles, RoleDTO>();
            CreateMap<RoleDTO, Roles>();
            CreateMap<EditRoleDTO, Roles>();
            CreateMap<Roles, EditRoleDTO>();
            CreateMap<Roles, CreateRoleDTO>();
            CreateMap<CreateRoleDTO, Roles>();

            CreateMap<Activities, ActivityDTO>();
            CreateMap<ActivityDTO, Activities>();

            CreateMap<Organization, OrganizationDTO>();

            CreateMap<Testimonials, TestimonialDTO>();
            CreateMap<TestimonialDTO, Testimonials>();

            CreateMap<Comments, CommentDTO>();
            CreateMap<CommentDTO, Comments>(); 
            CreateMap<Comments, CommentGetAllDTO>();
            CreateMap<CommentGetAllDTO, Comments>();
        }
    }
}
