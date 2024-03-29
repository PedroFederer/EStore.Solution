﻿using MyShopping.Models.EFModels;
using MyShopping.Models.Infra;
using MyShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyShopping.Controllers
{
    public class MembersController : Controller
    {
		// GET: Members
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}
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
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(LoginVm vm)
		{
			if (!ModelState.IsValid) { return View(vm); }
			try
			{
				ValidLogin(vm);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(vm);
			}

			var processResult = ProcessLogin(vm);

			Response.Cookies.Add(processResult.Cookie);

			return Redirect(processResult.ReturnUrl);
		}
		public ActionResult Logout()
		{
			Session.Abandon();
			FormsAuthentication.SignOut();
			return Redirect("/Members/Login");
		}
		private void ValidLogin(LoginVm vm)
		{
			var db = new AppDbContext();

			//根據帳號取得Member
			var member = db.Members.FirstOrDefault(p => p.Account == vm.Account);
			if (member == null)
			{
				throw new Exception("帳號或密碼錯誤"); //原則上不要告知細節
			}

			//檢查是否已經確認
			if (member.IsConfirmed == false)
			{
				throw new Exception("您尚未開通會員資格，請先收確認信，並點選信裡的連結，完成認證，才能登入本網站");
			}

			//將vm裡的密碼先雜湊之後，再與db裡的密碼比對
			var salt = HashUtility.GetSalt();
			var hashedPassword = HashUtility.ToSHA256(vm.Password, salt);

			if (string.Compare(member.EncryptedPassword, hashedPassword, true) != 0)
			{
				throw new Exception("帳號或密碼錯誤");
			}
		}
		private (string ReturnUrl, HttpCookie Cookie) ProcessLogin(LoginVm vm)
		{
			var rememberMe = false; //如果LoginVm有RememberMe屬性，記得要設定(=vm.rememberme)
			var account = vm.Account;
			var roles = string.Empty; //在本範例，沒有用到角色權限，所以存入空白

			//建立一張認證票
			var ticket =
				new FormsAuthenticationTicket(
					1,  //版本別，沒特別用處
					account,
					DateTime.Now, //發行日
					DateTime.Now.AddDays(2), //到期日
					rememberMe, //是否續存
					roles,  //userdata
					"/" //cookie位置
					);

			//將它加密
			var value = FormsAuthentication.Encrypt(ticket);

			//存入Cookie
			var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

			//取得return Url
			var url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數，沒有用處

			return (url, cookie);
		}

		[Authorize]
		public ActionResult EditProfile()
		{
			var currentUserAccount = User.Identity.Name;
			var vm = GetMemberProfile(currentUserAccount);

			return View(vm);
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditProfile(EditProfileVm vm)
		{
			var currentUserAccount = User.Identity.Name;
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			try
			{
				UpdateProfile(vm, currentUserAccount);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(vm);
			}
			return RedirectToAction("Index"); //回到會員中心頁
		}
		private void UpdateProfile(EditProfileVm vm, string account)
		{
			//利用vm.Id去資料庫取得Member
			var db = new AppDbContext();
			var memberInDb = db.Members.FirstOrDefault(p => p.Id == vm.Id);

			//如果這筆紀錄與目前使用者不符，就拒絕
			if (memberInDb.Account != account)
			{
				throw new Exception("您沒有權限修改別人的資料");
			}

			memberInDb.Name = vm.Name;
			memberInDb.Email = vm.Email;
			memberInDb.Mobile = vm.Mobile;

			db.SaveChanges();
		}

		private EditProfileVm GetMemberProfile(string account)
		{
			var db = new AppDbContext();

			var member = db.Members.FirstOrDefault(p => p.Account == account);
			if (member == null)
			{
				throw new Exception("帳號不存在");
			}

			var vm = member.ToEditProfileVm();

			return vm;
		}
		[Authorize]
		public ActionResult EditPassword()
		{
			return View();
		}
		[Authorize]
		[HttpPost]
		public ActionResult EditPassword(EditPasswordVm vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			try
			{
				var currentAccount = User.Identity.Name;
				ChangePassword(vm, currentAccount);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(vm);
			}
			return RedirectToAction("Index");
		}
		private void ChangePassword(EditPasswordVm vm, string account)
		{
			var db = new AppDbContext();
			var memberInDb = new AppDbContext().Members.FirstOrDefault(p => p.Account == account);
			if (memberInDb == null)
			{
				throw new Exception("帳號不存在");
			}
			var salt = HashUtility.GetSalt();

			//判斷書入的原始密碼是否正確
			var hashedOrigPassword = HashUtility.ToSHA256(vm.OriginalPassword, salt);
			if (string.Compare(memberInDb.EncryptedPassword, hashedOrigPassword, true) != 0)
			{
				throw new Exception("原始密碼不正確");
			}

			//將新密碼雜湊
			var hashedPassword = HashUtility.ToSHA256(vm.Password, salt);

			//更新紀錄
			memberInDb.EncryptedPassword = hashedPassword;
			db.SaveChanges();
		}
	}
}