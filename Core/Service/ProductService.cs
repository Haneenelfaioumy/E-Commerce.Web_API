using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using ServiceAbstraction;
using Shared.DataTransferObjects;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDTo>> GetAllBrandsAsync()
        {
            var Repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var Brands = await Repo.GetAllAsync();
            var BrandsDto = _mapper.Map<IEnumerable<ProductBrand> , IEnumerable<BrandDTo>>(Brands);
            return BrandsDto;
        }

        public async Task<IEnumerable<ProductDTo>> GetAllProductsAsync()
        {
            var Products = await _unitOfWork.GetRepository<Product ,  int>().GetAllAsync();
            return _mapper.Map<IEnumerable<Product> ,IEnumerable<ProductDTo>>(Products);
        }

        public async Task<IEnumerable<TypeDTo>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var TypesDto = _mapper.Map<IEnumerable<ProductType> , IEnumerable<TypeDTo>>(Types);
            return TypesDto;
        }

        public async Task<ProductDTo> GetProductByIdAsync(int Id)
        {
            var Product = await _unitOfWork.GetRepository<Product , int>().GetByIdAsync(Id);
            return _mapper.Map<Product , ProductDTo>(Product);
        }
    }
}
