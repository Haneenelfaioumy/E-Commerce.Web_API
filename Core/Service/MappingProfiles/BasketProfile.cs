using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models.BasketModule;
using Shared.DataTransferObjects.BasketModuleDTos;

namespace Service.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTo>().ReverseMap();
            CreateMap<BasketItem , BasketItemDTo>().ReverseMap();
        }
    }
}
