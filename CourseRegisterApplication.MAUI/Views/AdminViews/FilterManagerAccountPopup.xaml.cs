using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class FilterManagerAccountPopup : Popup
{
	public FilterManagerAccountPopup(FilterManagerAccountViewModel filterManagerAccountViewModel)
	{
		InitializeComponent();
		BindingContext = filterManagerAccountViewModel;
	}
}