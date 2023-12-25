using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MyShopping.Models.ViewModels
{
	public class CheckoutVm
	{
		[Display(Name = "收件人")]
		[Required]
		[MaxLength(30)]
		public string Receiver { get; set; }

		[Display(Name = "地址")]
		[Required]
		[MaxLength(200)]
		public string Address { get; set; }

		[Display(Name = "手機")]
		[Required]
		[MaxLength(10)]
		public string CellPhone { get; set; }
	}
}