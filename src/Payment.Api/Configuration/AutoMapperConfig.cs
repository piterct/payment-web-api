using AutoMapper;
using Payment.Api.ViewModels;
using Payment.Business.Dtos;
using Payment.Business.Models;

namespace Payment.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            #region Entities
            CreateMap<Seller, SellerRequest>().ReverseMap();
            CreateMap<OrderItem, OrderItemRequest>().ReverseMap();
            CreateMap<OrderRequest, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            #endregion

            #region Dtos
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<OrderItemDto, OrderItemRequest>().ReverseMap();
            CreateMap<OrderDto, OrderRequest>().ReverseMap();
            #endregion 
        }
    }
}
