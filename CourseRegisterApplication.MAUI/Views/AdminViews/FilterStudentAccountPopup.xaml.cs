using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class FilterStudentAccountPopup : Popup
{
	public FilterStudentAccountPopup(FilterStudentAccountViewModel filterStudentAccountViewModel)
	{
		InitializeComponent();
		BindingContext = filterStudentAccountViewModel;
	}
}