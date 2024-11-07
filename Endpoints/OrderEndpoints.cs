using AutoMapper;
using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Entities;
using MenuOnlineUdemy.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MenuOnlineUdemy.Endpoints
{
    public static class OrderEndpoints
    {
        public static RouteGroupBuilder MapOrders(this RouteGroupBuilder group)
        {

            group.MapGet("/", GetOrders);

            group.MapGet("/{id:int}", GetOrdersById);

            group.MapPost("/", CreateOrder);

            group.MapPut("/{id:int}", UpdateOrder);

            group.MapDelete("/{id:int}", DeleteOrder);

            return group;
        }

        static async Task<Ok<List<OrderDTO>>> GetOrders(IRepositoryOrders repository, IMapper mapper,
            int page = 1, int recordsByPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsByPage = recordsByPage };
            var orders = await repository.GetAll(pagination);
            var ordersDTO = mapper.Map<List<OrderDTO>>(orders);
            return TypedResults.Ok(ordersDTO);
        }

        static async Task<Results<Ok<OrderDTO>, NotFound>> GetOrdersById(IRepositoryOrders repository, int id
            , IMapper mapper)
        {
            var order = await repository.GetById(id);

            if (order == null)
            {
                return TypedResults.NotFound();
            }

            var orderDTO = mapper.Map<OrderDTO>(order);

            return TypedResults.Ok(orderDTO);
        }

        static async Task<Created<OrderDTO>> CreateOrder(CreateOrderDTO createOrderDTO, IRepositoryOrders repository
            , IMapper mapper)
        {
            var order = mapper.Map<Order> (createOrderDTO);

            var id = await repository.Create(order);

            var orderDTO = mapper.Map<OrderDTO>(order);

            return TypedResults.Created($"/orders/{id}", orderDTO);
        }

        static async Task<Results<NoContent, NotFound>> UpdateOrder(int id, CreateOrderDTO createOrderDTO, IRepositoryOrders repository
            , IMapper mapper)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            var order = mapper.Map<Order>(createOrderDTO);
            order.Id = id;

            await repository.Update(order);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> DeleteOrder(int id, IRepositoryOrders repository)
        {
            var exists = await repository.IfExists(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            return TypedResults.NoContent();
        }

    }
}
