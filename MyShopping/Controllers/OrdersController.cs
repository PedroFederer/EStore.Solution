using MyShopping.Entities;
using MyShopping.Models.ViewModels;
using MyShopping.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopping.Controllers
{
    public class OrdersController : Controller
    {
		// GET: Orders
		public ActionResult Index()
		{
			var orders = new OrderRepository().GetAll();
			List<OrderVM> orderVMs = ConvertToVM(orders);

			return View(orderVMs);
		}

		private List<OrderVM> ConvertToVM(List<Order> orders)
		{
			List<OrderVM> orderVMs = new List<OrderVM>();
			foreach (var order in orders)
			{
				OrderVM orderVM = new OrderVM();

				orderVM.OrderTime = order.OrderTime;
				orderVM.MemberAccount = order.Member.Account;

				orderVM.Total = CalcTotal(order);

				orderVMs.Add(orderVM);
			}

			return orderVMs;
		}
		private int CalcTotal(Order order)
		{
			int total = 0;
			foreach (var orderItem in order.OrderItems)
			{
				total += orderItem.Qty * orderItem.Product.Price;
			}
			return total;
		}
	}
}