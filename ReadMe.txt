﻿[v]add efmodels

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

[v]實作 修改個人基本資料-建立會員中心頁
	add Models/ViewModels/EditProfileVm.cs
	add Models/ViewModels/MemberExts class,擴充方法 ToEditProfileVm(Member)

	modify MembersController
		add Index action 會員中心頁，在登入成功之後會導向此頁
			add Views/Members/Index.cshtml（空白範本），填入二個超連結
		修改web.config裡的defaulturl
		add EditProfile action(Get,POST)

[v]del Views/Home/About.cshtml,Views/Home/Contact.cshtml

[v] modify_Layout.cshtml, add LOGO

[v]實作 變更密碼
	add Models/ViewModels/EditPasswordVm.cs
	modify MemberController
		add EditPassword action(Get, POST)

[V] 建立商品頁
	add ProductIndexVm.cs
	add ProductsController(新增空白controller), add Index action
		add Index.cshtml(list)
		add Create.cshtml, Edit.cshtml, Delete.cshtml

[V] 建立網站首頁
	add Home/Index.cshtml

[V] 調整Layout及新增購物車與結帳
	add CartVm.cs, CartItemVm, 顯示一筆購物車資料及其明細
	add CartController, 實作將商品加入購物車的功能
		AddItem action, 將商品加入購物車
		Info action, 顯示購物車內容，此時還沒實作數量的增減功能

	modify app_start/RouteConfig.cs, 預設為 Product/Index
	modify _Layout.cshtml, 加入購物車的連結
	modify CartController
		add UpdateItem action
	add Cart/ Info view page(用list範本, model是CartVm)
	add enum OrderStatus
	add CheckoutVm.cs
	modify CartController, add Checkout action
	add Checkout view page(用create範本, model是CheckoutVm)
	add ConfirmCheckout view page(empty template)

[V] Order
