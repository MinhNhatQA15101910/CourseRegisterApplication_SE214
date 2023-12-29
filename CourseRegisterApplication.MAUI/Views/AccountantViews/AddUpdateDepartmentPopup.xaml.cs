using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateDepartmentPopup : Popup
{
	public AddUpdateDepartmentPopup(AddUpdateDepartmentViewModel addUpdateDepartmentViewModel)
	{
		InitializeComponent();
		BindingContext = addUpdateDepartmentViewModel;
	}
}