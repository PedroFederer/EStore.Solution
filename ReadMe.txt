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
[v]	實作 新會員 Email確認功能
	會員請用的url: /Members/ActiveRegister? memberId=99&confirmCode=tttttttttttttttt
	modify MembersController
		add ActiveRegister(memberId, confirmCode)
	add Views/Members/ActiveRegister.cshtml
[v]實作 登入/登出網站
	只有帳密正確且已正式開通的會員才允許登入，實作之前，請先個別建立一個已/未開通的會員紀錄，方便測試
	modify web.config,add Authenthcation node
	add Models/ViewModels/LoginVm.cs
	modify MembersConntroller
		add Login action(Get,POST)
		add Logout action(get only)
	modify_layout,加入 login/logout link
	驗證：目前在沒登入時，會自動判斷權限，無法檢視About page;登入/登出功能實作