using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.DataTransferObjects.ProductModuleDTos;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        // Get All Products
        Task<PaginatedResult<ProductDTo>> GetAllProductsAsync(ProductQueryParams queryParams);
        // Get Product By Id
        Task<ProductDTo> GetProductByIdAsync(int id);

        // Get All Types
        Task<IEnumerable<TypeDTo>> GetAllTypesAsync();

        // Get All Brands
        Task<IEnumerable<BrandDTo>> GetAllBrandsAsync();
    }
}
