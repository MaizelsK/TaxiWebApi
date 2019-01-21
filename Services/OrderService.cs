using DataAccess;
using DTO;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService
    {
        private readonly TaxiContext _context;

        public OrderService(TaxiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderInfoDto>> GetOrdersInfoesAsync(int userId = 0)
        {
            IEnumerable<OrderInfoDto> orderDtos;

            if (userId > 0)
            {
                orderDtos = _context.Orders.Include(order => order.Creator)
                                              .Where(order => order.UserId == userId)
                                              .Select(order => new OrderInfoDto
                                              {
                                                  StartPoint = order.StartPoint,
                                                  EndPoint = order.EndPoint,
                                                  OrderId = order.Id,
                                                  Price = order.Price,
                                                  Status = Enum.GetName(typeof(OrderStatus), order.Status),
                                                  Username = order.Creator.Username
                                              });
            }
            else
            {
                orderDtos = _context.Orders.Include(order => order.Creator)
                                              .Select(order => new OrderInfoDto
                                              {
                                                  StartPoint = order.StartPoint,
                                                  EndPoint = order.EndPoint,
                                                  OrderId = order.Id,
                                                  Price = order.Price,
                                                  Status = Enum.GetName(typeof(OrderStatus), order.Status),
                                                  Username = order.Creator.Username
                                              });
            }

            return await Task.FromResult(orderDtos);
        }

        public async Task<Order> CreateOrder(NewOrderDto dto, int userId)
        {
            var newOrder = new Order
            {
                StartPoint = dto.StartPoint,
                EndPoint = dto.EndPoint,
                Price = dto.Price,
                UserId = userId,
                Status = OrderStatus.Created
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return newOrder;
        }

        public async Task<Order> OrderOnRoadAsync(int orderId)
        {
            var order = await _context.Orders
                        .Where(ord => ord.Status == OrderStatus.Created)
                        .SingleOrDefaultAsync(ord => ord.Id == orderId);

            if (order == null)
                return await Task.FromResult(order);

            order.Status = OrderStatus.OnRoad;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> FinishOrderAsync(int orderId)
        {
            var order = await _context.Orders
                        .Where(ord=>ord.Status == OrderStatus.OnRoad)
                        .SingleOrDefaultAsync(ord => ord.Id == orderId);

            if (order == null)
                return await Task.FromResult(order);

            order.Status = OrderStatus.Done;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
