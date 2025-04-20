using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]  // BaseUrl/api/Products
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        // Get All Products
        // GET BaseUrl/api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTo>>> GetAllProducts
             (int? BrandId , int? TypeId ,ProductSortingOptions sortingOptions)
        {
            var Products = await _serviceManager.ProductService.GetAllProductsAsync(BrandId , TypeId , sortingOptions);
            return Ok(Products);
        }

        // Get Product By Id
        // GET BaseUrl/api/Products/id
        [HttpGet ("{id:int}")]
        public async Task<ActionResult<ProductDTo>> GetProduct(int id)
        {
            var Product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        // Get All Types
        // GET BaseUrl/api/Products/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTo>>> GetTypes()
        {
            var Types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }

        // Get All Brands
        // GET BaseUrl/api/Products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTo>>> GetBrands()
        {
            var Brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }
    }
}
