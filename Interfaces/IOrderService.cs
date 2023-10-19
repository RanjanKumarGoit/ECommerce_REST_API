using System.Collections.Generic;
using trying.Controllers.Dto;
using trying.Model;

namespace trying.Interfaces
{
    public interface IOrderService
    {
        bool CreateOrder(int userId);
        List<OrderItem> GetOrderItemsForUser(int userId);
    }

}
