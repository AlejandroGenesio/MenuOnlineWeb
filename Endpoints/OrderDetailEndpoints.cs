using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MenuOnlineUdemy.Endpoints
{
    public static class OrderDetailEndpoints
    {
        public static RouteGroupBuilder MapOrderDetails(this RouteGroupBuilder group)
        {

            group.MapPost("/", CreateOrderDetail);

            return group;
        }
        /*
        static async Task<Results<Created<VariantDTO>, NotFound>> CreateVariant(int productId, CreateVariantDTO createVariantDTO
            , IRepositoryVariants repositoryVariants
            , IRepositoryProducts repositoryProducts, IMapper mapper){
            if(! await repositoryProducts.IfExists(productId)){
                return TypedResults.NotFound();}

            var variant = mapper.Map<Variant> (createVariantDTO);
            variant.ProductId = productId;

            var id = await repositoryVariants.Create(variant);

            var variantDTO = mapper.Map<VariantDTO>(variant);

            return TypedResults.Created($"/variants/{id}", variantDTO);
        }
         */

        static async Task<Results<Created<OrderDetailDTO>, NotFound>> CreateOrderDetail(int orderId, CreateOrderDetailDTO createOrderDetailDTO
            , IRepositoryOrderDetails repositoryOrderDetails
            , IRepositoryOrders repositoryOrders, IMapper mapper)
        {
            if(! await repositoryOrders.IfExists(orderId))
            {
                return TypedResults.NotFound();
            }

            var orderDetail = mapper.Map<OrderDetails> (createOrderDetailDTO);
            orderDetail.OrderId = orderId;

            var id = await repositoryOrderDetails.Create(orderDetail);

            var orderDetailDTO = mapper.Map<OrderDetailDTO>(orderDetail);

            return TypedResults.Created($"/orderdetails/{id}", orderDetailDTO);
        }
    }
}
