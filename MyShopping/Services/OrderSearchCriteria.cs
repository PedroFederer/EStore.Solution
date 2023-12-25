using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopping.Services
{
	public class OrderSearchCriteria
	{
		public int? MemberId { get; set; }
		public string Name { get; set; }
	}
}