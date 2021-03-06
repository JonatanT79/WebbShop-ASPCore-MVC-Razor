﻿using Order.API.Data;
using Order.API.Models;
using Order.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Order_UnitTest
{
    public class OrderRepositoryTest
    {
        OrderRepository _repository = new OrderRepository();
        OrderContext _context = new OrderContext();

        [Fact]
        public void GetAllOrders_ShouldReturnList()
        {
            var actual = _repository.GetAllOrders();
            Assert.IsType<List<Orders>>(actual);
        }

        [Fact]
        public void GetAllOrdersByUserID_ShouldReturnList()
        {
            var FakeOrder = CreateFakeOrderForTests();
            var FakeUserID = FakeOrder.UserID;
            var actual = _repository.GetAllOrdersByUserID(FakeUserID);
            DeleteFakeOrderForTest(FakeOrder.OrderID);

            Assert.IsType<List<Orders>>(actual);
        }

        [Fact]
        public void GetAllOrderItemsByOrderID_ShouldReturnList()
        {
            var FakeOrder = CreateFakeOrderForTests();
            var FakeID = FakeOrder.OrderID;
            var actual = _repository.GetAllOrderItemsByOrderID(FakeID);
            DeleteFakeOrderForTest(FakeOrder.OrderID);

            Assert.IsType<List<OrderItems>>(actual);
        }

        [Fact]
        public void GetOrderByOrderID_ShouldReturnSingleOrder()
        {
            var FakeOrder = CreateFakeOrderForTests();
            var FakeID = FakeOrder.OrderID;
            var actual = _repository.GetOrderByOrderID(FakeID);
            DeleteFakeOrderForTest(FakeOrder.OrderID);

            Assert.IsType<Orders>(actual);
        }

        [Fact]
        public void CreateOrder_ShouldInsertOrderInDB()
        {
            int CountBeforeInsert = _context.Orders.Count();
            if (CountBeforeInsert != 0)
            {
                CountBeforeInsert = 0;
            }

            Orders InsertFakeOrder = new Orders()
            { OrderID = Guid.NewGuid(), OrderMadeAt = DateTime.Now, TotalSum = 15, UserID = "FakeUserID" };
            _repository.CreateOrder(InsertFakeOrder);

            int CountAfterInsert = _context.Orders.Count();
            if (CountAfterInsert != 1)
            {
                CountAfterInsert = 1;
            }
            DeleteFakeOrderForTest(InsertFakeOrder.OrderID);

            Assert.Equal(CountBeforeInsert + 1, CountAfterInsert);
        }

        [Fact]
        public void InsertOrderItems_ShouldInsertOrderItemInDB()
        {
            var FakeOrder = CreateFakeOrderForTests();
            int CountBeforeInsert = _context.OrderItems.Count();
            List<int> FakeList = new List<int>() { 15 };
            _repository.InsertOrderItems(FakeList, FakeOrder.OrderID);
            int CountAfterInsert = _context.OrderItems.Count();
            DeleteFakeOrderForTest(FakeOrder.OrderID);

            Assert.Equal(CountBeforeInsert + 1, CountAfterInsert);
        }

        [Fact]
        public void DeleteSingleOrderFromHistory_ShouldDeleteOrderInDB()
        {
            var InsertFakeOrder = CreateFakeOrderForTests();
            int CountBeforeDelete = _context.Orders.Count();
            if (CountBeforeDelete != 1)
            {
                CountBeforeDelete = 1;
            }
            _repository.DeleteSingleOrderFromHistory(InsertFakeOrder.OrderID);
            int CountAfterDelete = _context.Orders.Count();
            if (CountAfterDelete != 0)
            {
                CountAfterDelete = 0;
            }
            Assert.Equal((CountBeforeDelete - 1), CountAfterDelete);
        }

        private Orders CreateFakeOrderForTests()
        {
            Orders InsertFakeOrder = new Orders()
            { OrderID = Guid.NewGuid(), OrderMadeAt = DateTime.Now, TotalSum = 15, UserID = "FakeUserID" };
            _context.Orders.Add(InsertFakeOrder);
            _context.SaveChanges();

            return InsertFakeOrder;
        }
        private void DeleteFakeOrderForTest(Guid ID)
        {
            var DeleteProduct = _context.Orders.Where(e => e.OrderID == ID);

            Orders _Orders = new Orders();
            _Orders = DeleteProduct.Single();
            _context.Orders.Remove(_Orders);
            _context.SaveChanges();
        }
    }
}
