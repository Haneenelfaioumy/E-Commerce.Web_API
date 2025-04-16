using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        // Get All Products
        Task<IEnumerable<ProductDTo>> GetAllProductsAsync();

        // Get Product By Id
        Task<ProductDTo> GetProductByIdAsync(int id);

        // Get All Types
        Task<IEnumerable<TypeDTo>> GetAllTypesAsync();

        // Get All Brands
        Task<IEnumerable<BrandDTo>> GetAllBrandsAsync();
    }
}
