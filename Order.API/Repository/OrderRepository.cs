﻿using Order.API.Data;
using Order.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        readonly OrderContext _context = new OrderContext();
        public List<Orders> GetAllOrders()
        {
            var ListOfOrders = _context.Orders.ToList();
            return ListOfOrders;
        }
        public Orders GetOrderByID(Guid ID)
        {
            var FindOrder = _context.Orders.Where(e => e.ID == ID);
            var GetOrder = FindOrder.Single();

            return GetOrder;
        }
    }
}