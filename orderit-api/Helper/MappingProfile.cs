using AutoMapper;
using orderit_api.Dto;
using orderit_api.Models;

namespace orderit_api.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Brand, BrandDto>();

            CreateMap<Category, CategoryDto>();
            
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            
            CreateMap<Store,StoreDto>();
            CreateMap<StoreDto, Store>();
            
            CreateMap<Salesperson, SalespersonDto>();
            CreateMap<SalespersonDto, Salesperson>();
            
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();   
            
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetailDto, OrderDetail>();
        }
    }
}