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
			return View();
		}
		public ActionResult ValidateAccount(RegisterVm vm)
		{
			var db = new AppDbContext();
			var memberInDb = db.Members.FirstOrDefault(p => p.Account == vm.Account);

			bool result = memberInDb == null;
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}