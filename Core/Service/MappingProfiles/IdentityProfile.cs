using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models.IdentityModule;

namespace Service.MappingProfiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address , Address>().ReverseMap();
        }
    }
}
