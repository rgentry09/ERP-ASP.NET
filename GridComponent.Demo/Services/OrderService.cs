using GridBlazor.Demo.Shared.Models;
using GridComponent.Demo.Models;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using System;

namespace GridComponent.Demo.Services
{
    public class OrderService : IOrderService
    {

        private readonly NorthwindDbContext _context;

        public OrderService(NorthwindDbContext context)
        {
            _context = context;
        }

        public ItemsDTO<Order> GetOrdersGridRows(Action<IGridColumnCollection<Order>> columns,
            QueryDictionary<StringValues> query)
        {
            var repository = new OrdersRepository(_context);
            var server = new GridServer<Order>(repository.GetAll(), new QueryCollection(query), 
                true, "ordersGrid", columns)
                    .Sortable()
                    .WithPaging(10)
                    .Filterable()
                    .WithMultipleFilters()
                    .Searchable(true, false);
            
            // return items to displays
            var items = server.ItemsToDisplay;

            // uncomment the following lines are to test null responses
            //items = null;
            //items.Items = null;
            //items.Pager = null;
            return items;
        }

        public ItemsDTO<Order> GetOrdersGridRows(QueryDictionary<StringValues> query)
        {
            var repository = new OrdersRepository(_context);
            var server = new GridServer<Order>(repository.GetAll(), new QueryCollection(query),
                true, "ordersGrid", null).AutoGenerateColumns();

            // return items to displays
            return server.ItemsToDisplay;
        }
    }

    public interface IOrderService
    {
        ItemsDTO<Order> GetOrdersGridRows(Action<IGridColumnCollection<Order>> columns, QueryDictionary<StringValues> query);
        ItemsDTO<Order> GetOrdersGridRows(QueryDictionary<StringValues> query);
    }
}
