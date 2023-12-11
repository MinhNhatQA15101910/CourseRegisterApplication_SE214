using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateProvincePopup : Popup
{
	public AddUpdateProvincePopup(AddUpdateProvinceViewModel addUpdateProvinceViewModel)
	{
		InitializeComponent();
		BindingContext = addUpdateProvinceViewModel;
	}
}