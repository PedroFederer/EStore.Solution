[v]add efmodels
�|���t��
[v]add���U�|���\��
	add Models/Infra/HashUtility.cs
	add AppSettings,<add key="salt" value="ar!Zu@#$D691RR"/>
	add Models/ViewModels/RegisterVm.cs
	add RegisterExts class,�X�R��k ToEFModel(RegisterVM)
	add Controllers/MembersController
		add Register action(Get,Post)
		add Views/Members/Register.cshtml,RegisterConfirm.cshtml
		modify_Layout.cshtml, add ValidateAccount, add Register link
[v] add RegisterConfirm.cshtml
[v] add RegieterMember
[v] ��@ �s�|�� Email�T�{�\��
	�|���ХΪ�url: /Members/ActiveRegister? memberId=99&confirmCode=tttttttttttttttt
	modify MembersController
		add ActiveRegister(memberId, confirmCode)
	add Views/Members/ActiveRegister.cshtml
