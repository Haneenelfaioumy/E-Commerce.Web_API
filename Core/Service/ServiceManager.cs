using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;

namespace Service
{
    public class ServiceManager(IUnitOfWork unitOfWork , 
                                IMapper mapper , 
                                IBasketRepository basketRepository ,
                                UserManager<ApplicationUser> userManager ,
                                IConfiguration configuration) : IServiceManager
    {
        private readonly Lazy<IProductService> _LazyProductService =
                             new Lazy<IProductService>(() => new ProductService(unitOfWork , mapper));

        private readonly Lazy<IBasketService> _LazyBasketService =
                             new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));

        private readonly Lazy<IAuthenticationService> _LazyAuthenticationService =
                             new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager , configuration));
        public IProductService ProductService => _LazyProductService.Value;
        public IBasketService BasketService => _LazyBasketService.Value;
        public IAuthenticationService AuthenticationService => _LazyAuthenticationService.Value;
    }
}
