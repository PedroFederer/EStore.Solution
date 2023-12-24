using MyShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopping.Models.ViewModels
{
	public static class MemberExts
	{
		public static EditProfileVm ToEditProfileVm(this Member member)
		{
			return new EditProfileVm
			{
				Id = member.Id,
				Email = member.Email,
				Name = member.Name,
				Mobile = member.Mobile,
			};
		}
	}
}