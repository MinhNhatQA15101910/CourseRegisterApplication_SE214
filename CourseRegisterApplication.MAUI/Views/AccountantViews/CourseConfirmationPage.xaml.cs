using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class CourseConfirmationPage : ContentPage
{
	public CourseConfirmationPage(CourseConfirmationViewModel courseConfirmationViewModel)
	{
		InitializeComponent();
		BindingContext = courseConfirmationViewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();

        //(BindingContext as CourseConfirmationViewModel).GetCourseRegistrationFormsCommand.Execute(null);
    }
}