using Dapper;
using MyShopping.Entities;
using MyShopping.Services;
using MyShopping.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopping.Repositories
{
	public class OrderRepository
	{
		public List<Order> GetAll()
		{
			string sql = @"SELECT O.*, OI.*, M.*, P.*
FROM OrderItems as OI
INNER JOIN Orders as O ON O.Id = OI.OrderId
INNER JOIN Members as M ON M.Id = O.MemberId
INNER JOIN Products as P ON P.Id = OI.ProductId
ORDER BY O.ID DESC";

			Dictionary<int, Order> orders = new Dictionary<int, Order>();

			using (var conn = new SqlDb().GetConnection("default"))
			{
				var dummy = conn.Query<Order, OrderItem, Member, Product, Order>(sql, (order, orderItem, member, product) =>
				{
					if (!orders.TryGetValue(order.Id, out Order o))
					{
						order.Member = member;
						orderItem.Product = product;
						order.OrderItems = new List<OrderItem>();
						order.OrderItems.Add(orderItem);

						orders.Add(order.Id, order);
					}
					else
					{
						orderItem.Product = product;
						o.OrderItems.Add(orderItem);
					}

					return o;
				});

			}

			return orders.Values.ToList();
		}

		public List<Member> GetMembers()
		{
			using (var conn = new SqlDb().GetConnection("default"))
			{
				string sql = "SELECT Id, Name FROM Members ORDER BY Id";
				var results = conn.Query<Member>(sql).ToList();
				return results;
			};
		}
		public List<Order> Search(OrderSearchCriteria criteria)
		{
			Dictionary<int, Order> orders = new Dictionary<int, Order>();

			object obj = new { memberId = (int?)null, name = criteria.Name };

			using (var conn = new SqlDb().GetConnection("default"))
			{
				var dummy = conn.Query<Order, OrderItem, Member, Product, Order>("dbo.SearchOrderItems @MemberId = @memberId , @Name = @name", (order, orderItem, member, product) =>
				{
					if (!orders.TryGetValue(order.Id, out Order o))
					{
						order.Member = member;
						orderItem.Product = product;
						order.OrderItems = new List<OrderItem>();
						order.OrderItems.Add(orderItem);

						orders.Add(order.Id, order);
					}
					else
					{
						orderItem.Product = product;
						o.OrderItems.Add(orderItem);
					}

					return o;

				}, obj);
				return orders.Values.ToList();
			}

		}
	}
}