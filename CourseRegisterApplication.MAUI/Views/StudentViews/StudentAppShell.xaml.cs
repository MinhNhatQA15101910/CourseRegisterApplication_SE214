using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;

namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class StudentAppShell : Shell
{
	public StudentAppShell(StudentAppShellViewModel studentAppShellViewModel)
	{
		InitializeComponent();
		BindingContext = studentAppShellViewModel;

		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(CourseRegistrationInfoPage), typeof(CourseRegistrationInfoPage));
    }
    protected override void OnAppearing()
	{
		base.OnAppearing();

		(BindingContext as StudentAppShellViewModel).GetCurrentStudentCommand.Execute(null);
	}
}