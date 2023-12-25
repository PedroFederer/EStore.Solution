using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopping.Models.ViewModels
{
	public class OrderVM
	{

		public DateTime OrderTime { get; set; }
		public string MemberAccount { get; set; }
		public int Total { get; set; }

	}
}