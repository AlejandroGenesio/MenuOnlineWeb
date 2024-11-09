using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;

namespace MenuOnlineUdemy.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateProductDTO, Product>();
            CreateMap<Product, ProductDTO>();

            CreateMap<CreateVariantDTO, Variant>();
            CreateMap<Variant, VariantDTO>();

            CreateMap<CreateModifierGroupDTO, ModifierGroup>();
            CreateMap<ModifierGroup, ModifierGroupDTO>();

            CreateMap<CreateModifierExtraDTO, ModifierExtra>();
            CreateMap<ModifierExtra, ModifierExtraDTO>();

            CreateMap<CreateImageDTO, Image>()
                .ForMember(x => x.File, options => options.Ignore());
            CreateMap<Image, ImageDTO>();

            CreateMap<CreateOrderDTO, Order>();
            CreateMap<Order, OrderDTO>();

            CreateMap<AssignProductModifierGroup, ProductModifierGroup>();
        }
    }
}
