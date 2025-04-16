using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models;
using Shared.DataTransferObjects;

namespace Service.MappingProfiles
{
    public class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTo>()
                .ForMember(dist => dist.BrandName, Options => Options.MapFrom(Src => Src.ProductBrand.Name))
                .ForMember(dist => dist.TypeName, Options => Options.MapFrom(Src => Src.ProductType.Name));

            CreateMap<ProductType, TypeDTo>();

            CreateMap<ProductBrand, BrandDTo>();
        }
    }
}
