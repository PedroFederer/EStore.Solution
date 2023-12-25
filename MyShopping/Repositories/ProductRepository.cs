using Dapper;
using MyShopping.Entities;
using MyShopping.Models.ViewModels;
using MyShopping.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace MyShopping.Repositories
{
	public class ProductRepository
	{
		public List<Product> GetAll()
		{
			string sql = @"SELECT P.*, C.*
FROM Products as P
INNER JOIN Categories as C ON C.Id=P.CategoryId";

			using (var conn = new SqlDb().GetConnection("default"))
			{
				return conn.Query<Product, Category, Product>(sql, (p, c) =>
				{
					p.Category = c;
					return p;
				})
					.ToList();
			}
		}
		public void Create(Product product)
		{
			string sql = @"INSERT INTO Products (Name, CategoryId, Price)
                   VALUES (@Name, @CategoryId, @Price)";

			using (var conn = new SqlDb().GetConnection("default"))
			{
				conn.Execute(sql, new
				{
					Name = product.Name,
					CategoryId = product.CategoryId,
					Price = product.Price,
				});
			}
		}

	}

}