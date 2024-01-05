using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AvailableCourseManagementPage : ContentPage
{
	public AvailableCourseManagementPage(AvailableCourseManagementViewModel availableCourseManagementViewModel)
	{
		InitializeComponent();
		BindingContext = availableCourseManagementViewModel;

		availableCourseManagementViewModel.GetSubjectsCommand.Execute(null);
	}
}