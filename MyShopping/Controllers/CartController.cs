using MyShopping.Models.EFModels;
using MyShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopping.Controllers
{
	public class CartController : Controller
	{   /// <summary>
		/// 將productId加入購物車
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		[Authorize]
		public ActionResult AddItem(int productId)
		{
			//todo將productId加入購物車
			string buyer = User.Identity.Name; //買家帳號
			AddToCart(buyer, productId, 1);  //加入購物車

			return new EmptyResult(); //沒傳回任何東西
		}

		[Authorize]
		public ActionResult Info()
		{
			string buyer = User.Identity.Name;
			CartVm cart = GetOrCreateCart(buyer);

			return View(cart);
		}

		[Authorize]
		public ActionResult UpdateItem(int productId, int newQty)
		{
			var buyer = User.Identity.Name;
			newQty = newQty < 0 ? 0 : newQty;

			UpdateItemQty(buyer, productId, newQty);

			return new EmptyResult();
		}

		[Authorize]
		public ActionResult Checkout()
		{
			var buyer = User.Identity.Name;
			var cart = GetOrCreateCart(buyer);

			if (cart.AllowCheckout == false) ViewBag.ErrorMessage = "購物車是空的，無法進行結帳";

			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult Checkout(CheckoutVm vm)
		{
			if (!ModelState.IsValid) return View(vm);

			var buyer = User.Identity.Name;
			var cart = GetOrCreateCart(buyer);

			if (cart.AllowCheckout == false)
			{
				ModelState.AddModelError("", "購物車是空的，無法進行結帳");
				return View(vm);
			}

			ProcessCheckout(buyer, cart, vm);
			return View("ConfirmCheckout");
		}

		private void ProcessCheckout(string account, CartVm cart, CheckoutVm vm)
		{
			//建立訂單主檔明細檔
			CreateOrder(account, cart, vm);
			//清空購物車
			EmptyCart(account);
		}

		private void EmptyCart(string account)
		{
			var db = new AppDbContext();
			var cart = db.Carts.FirstOrDefault(c => c.MemberAccount == account);
			if (cart == null) return;
			db.Carts.Remove(cart);
			db.SaveChanges();
		}

		private void CreateOrder(string account, CartVm cart, CheckoutVm vm)
		{
			var db = new AppDbContext();

			var memberId = db.Members.First(m => m.Account == account).Id;

			var order = new Order
			{
				MemberId = memberId,
				Receiver = vm.Receiver,
				Address = vm.Address,
				CellPhone = vm.CellPhone,
				CreatedTime = DateTime.Now,
				Status = 1, //1:新訂單
				Total = cart.Total,
			};

			//新增訂單明細
			foreach (var item in cart.CartItems)
			{
				var orderItem = new OrderItem
				{
					ProductId = item.ProductId,
					ProductName = item.Product.Name,
					Price = item.Product.Price,
					Qty = item.Qty,
					SubTotal = item.SubTotal,
				};
				order.OrderItems.Add(orderItem);
			}
			db.Orders.Add(order);
			db.SaveChanges();
		}

		private void UpdateItemQty(string buyer, int productId, int newQty)
		{
			var cart = GetOrCreateCart(buyer);
			var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
			if (cartItem == null) return;//購物車根本沒收這筆商品，就不處理

			var db = new AppDbContext();
			if (newQty == 0)
			{//新數量為0，就刪除這筆資料
				var item = db.CartItems.Find(cartItem.Id);
				db.CartItems.Remove(item);
				db.SaveChanges();
				return;
			}
			else
			{
				var cartItemInDb = db.CartItems.FirstOrDefault(ci => ci.Id == cartItem.Id);
				cartItemInDb.Qty = newQty;
				db.SaveChanges();
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buyer"></param>
		/// <param name="productId"></param>
		/// <param name="qty"></param>
		private void AddToCart(string buyer, int productId, int qty)
		{
			//取得目前購物車主檔，若沒有，就立刻新增一筆並傳回
			CartVm cart = GetOrCreateCart(buyer);
			//加入購物車，若明細不存在就在新增一筆，若存在就更新數量
			AddCartItem(cart, productId, qty);
		}

		/// <summary>
		/// 加入購物車，若明細不存在就在新增一筆，若存在就更新數量
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="productId"></param>
		/// <param name="qty"></param>
		private void AddCartItem(CartVm cart, int productId, int qty)
		{
			var db = new AppDbContext();

			var cartItem = db.CartItems.FirstOrDefault(x => x.CartId == cart.Id && x.ProductId == productId);
			if (cartItem == null)
			{
				var newItem = new CartItem
				{
					CartId = cart.Id,
					ProductId = productId,
					Qty = qty
				};
				db.CartItems.Add(newItem);
				db.SaveChanges();
				return;
			}

			cartItem.Qty += qty;
			db.SaveChanges();
		}

		/// <summary>
		/// 取得目前購物車主檔，若沒有，就立刻新增一筆並傳回
		/// </summary>
		/// <param name="buyer"></param>
		/// <returns></returns>
		private CartVm GetOrCreateCart(string buyer)
		{
			var db = new AppDbContext();

			var cart = db.Carts.FirstOrDefault(x => x.MemberAccount == buyer);
			if (cart == null)
			{  //立刻新增一筆並傳回
				cart = new Cart { MemberAccount = buyer };
				db.Carts.Add(cart);
				db.SaveChanges();

				return new CartVm
				{
					Id = cart.Id,
					MemberAccount = cart.MemberAccount,
					CartItems = new List<CartItemVm>()
				};
			}
			return new CartVm
			{
				Id = cart.Id,
				MemberAccount = cart.MemberAccount,
				CartItems = cart.CartItems.Select(x => new CartItemVm
				{
					Id = x.Id,
					CartId = x.CartId,
					ProductId = x.ProductId,
					Product = new ProductIndexVM
					{
						Id = x.Product.Id,
						Name = x.Product.Name,
						Price = x.Product.Price,
					},
					Qty = x.Qty,
				})
			};


		}
	}
}