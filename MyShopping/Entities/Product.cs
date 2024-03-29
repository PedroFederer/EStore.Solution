﻿using MyShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopping.Entities
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public int Price { get; set; }
		public bool Status { get; set; }
		public string ProductImage { get; set; }
		public int Stock { get; set; }
	}
}