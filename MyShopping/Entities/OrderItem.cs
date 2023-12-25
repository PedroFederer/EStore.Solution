using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopping.Entities
{
	public class OrderItem
	{
		public int Id { get; set; }
		public Order Order { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public int Qty { get; set; }
	}
	public class Member
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class Order
	{
		public int Id { get; set; }
		public int MemberId { get; set; }
		public Member Member { get; set; }
		public DateTime OrderTime { get; set; }
		public List<OrderItem> OrderItems { get; set; }
	}
}