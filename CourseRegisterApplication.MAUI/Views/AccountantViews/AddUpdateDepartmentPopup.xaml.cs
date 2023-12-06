using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateDepartmentPopup : Popup
{
	public AddUpdateDepartmentPopup(AddUpdateDepartmentViewModel addUpdateDepartmentViewModel)
	{
		InitializeComponent();
		BindingContext = addUpdateDepartmentViewModel;
	}

    private void ClosePopup(object sender, TappedEventArgs e)
    {
		Close();
    }
}