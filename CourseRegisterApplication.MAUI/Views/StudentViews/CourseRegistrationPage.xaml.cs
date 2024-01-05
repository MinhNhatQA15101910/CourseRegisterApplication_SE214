using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;
namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class CourseRegistrationPage : ContentPage
{
	public CourseRegistrationPage(CourseRegistrationViewModel courseRegistrationViewModel)
	{
		InitializeComponent();
		BindingContext = courseRegistrationViewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as CourseRegistrationViewModel).GetSetupCommand.Execute(null);
    }
}