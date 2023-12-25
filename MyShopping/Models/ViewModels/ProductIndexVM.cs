using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MyShopping.Models.ViewModels
{
	public class ProductIndexVM
	{
		public int Id { get; set; }

		[Display(Name = "名稱")]
		public string Name { get; set; }

		[Display(Name = "分類名稱")]
		public string CategoryName { get; set; }
		[Display(Name = "售價")]
		public int Price { get; set; }


	}
}