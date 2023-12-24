[v]add efmodels
會員系統
[v]add註冊會員功能
	add Models/Infra/HashUtility.cs
	add AppSettings,<add key="salt" value="ar!Zu@#$D691RR"/>
	add Models/ViewModels/RegisterVm.cs
	add RegisterExts class,擴充方法 ToEFModel(RegisterVM)
	add Controllers/MembersController
		add Register action(Get,Post)
		add Views/Members/Register.cshtml,RegisterConfirm.cshtml
		modify_Layout.cshtml, add ValidateAccount, add Register link
[v] add RegisterConfirm.cshtml
[v] add RegieterMember