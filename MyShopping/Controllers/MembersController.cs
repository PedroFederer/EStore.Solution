using MyShopping.Models.EFModels;
using MyShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopping.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
		public ActionResult Register(RegisterVm vm)
		{
			if (!ModelState.IsValid) //沒有通過驗證
			{
				return View(vm);
			}
			try
			{
				RegisterMember(vm);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(vm);
			}

			return View("RegisterConfirm");
		}
		public ActionResult ValidateAccount(RegisterVm vm)
		{
			var db = new AppDbContext();
			var memberInDb = db.Members.FirstOrDefault(p => p.Account == vm.Account);

			bool result = memberInDb == null;
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		private void RegisterMember(RegisterVm vm)
		{
			var db = new AppDbContext();
			//判斷帳號是否已經存在
			var memberInDb = db.Members.FirstOrDefault(p => p.Account == vm.Account);
			if (memberInDb != null)
			{
				throw new Exception("帳號已經存在");
			}
			//將vm轉成Member
			var member = vm.ToEFModel();
			//叫用EF 寫入資料庫
			db.Members.Add(member);
			db.SaveChanges();

			// todo發出確認信
		}
		public ActionResult ActiveRegister(int memberId, string confirmCode)
		{
			if (memberId <= 0 || string.IsNullOrEmpty(confirmCode))
			{
				return View();
			}

			var db = new AppDbContext();

			//根據memberId, confirmCode取得Member
			var member = db.Members.FirstOrDefault(p => p.Id == memberId && p.ConfirmCode == confirmCode && p.IsConfirmed == false);

			if (member == null)
			{
				return View();
			}

			//將它更新為已確認
			member.IsConfirmed = true;
			member.ConfirmCode = null;
			db.SaveChanges();

			return View();
		}
	}
}