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
            CreateMap<Product, ProductDTO>()
                .ForMember(p => p.Images, entity => entity.MapFrom(p => 
                p.ProductImages.Select(ap => 
                    new ProductImageDTO { Id = ap.ImageId, 
                                          File = ap.Image.File})))
                .ForMember(p => p.Categories, entity => entity.MapFrom(p =>
                p.ProductCategories.Select(ap =>
                    new ProductCategoryDTO
                    {
                        Id = ap.CategoryId,
                        Name = ap.Category.Name})))
                .ForMember(p => p.ModifierGroups, entity => entity.MapFrom(p =>
                p.ProductModifierGroups.Select(ap =>
                    new ProductModifierGroupDTO
                    {
                        Id = ap.ModifierGroupId,
                        Label = ap.ModifierGroup.Label
                    })));


            CreateMap<CreateVariantDTO, Variant>();
            CreateMap<Variant, VariantDTO>();

            CreateMap<CreateModifierGroupDTO, ModifierGroup>();
            CreateMap<ModifierGroup, ModifierGroupDTO>()
                .ForMember(p => p.ModifierOptions, entity => entity.MapFrom(p =>
                p.ModifierGroupOptions.Select(ap =>
                    new ModifierOptionDTO
                    {
                        Id = ap.Id,
                        Description = ap.Description,   
                        Name = ap.Name,
                        Price = ap.Price
                    })));

            CreateMap<CreateModifierOptionDTO, ModifierOption>();
            CreateMap<ModifierOption, ModifierOptionDTO>();

            CreateMap<CreateImageDTO, Image>()
                .ForMember(x => x.File, options => options.Ignore());
            CreateMap<Image, ImageDTO>();

            CreateMap<CreateOrderDTO, Order>();
            CreateMap<Order, OrderDTO>();

            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();

            CreateMap<CreateOrderDetailDTO, OrderDetails>();
            CreateMap<OrderDetails, OrderDetailDTO>();
        }
    }
}
