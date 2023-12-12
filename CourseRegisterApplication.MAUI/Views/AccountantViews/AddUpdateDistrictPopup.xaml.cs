using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateDistrictPopup : Popup
{
	public AddUpdateDistrictPopup(AddUpdateDistrictViewModel addUpdateDistrictViewModel)
	{
		InitializeComponent();
		BindingContext = addUpdateDistrictViewModel;
    }
}