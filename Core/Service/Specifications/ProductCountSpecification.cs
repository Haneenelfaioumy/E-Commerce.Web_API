using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models.ProductModule;
using Shared;

namespace Service.Specifications
{
    class ProductCountSpecification : BaseSpecifications<Product, int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams) 
            : base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
                      &&
                        (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId)
                      &&
                        (string.IsNullOrWhiteSpace(queryParams.SearchValue) ||
                         P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
        }
    }
}
