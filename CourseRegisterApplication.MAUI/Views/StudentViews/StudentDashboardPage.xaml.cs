namespace CourseRegisterApplication.MAUI.Views.StudentViews;
using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;

public partial class StudentDashboardPage : ContentPage
{
	public StudentDashboardPage(StudentDashboardViewModel studentDashboardViewModel)
	{
		InitializeComponent();
		BindingContext = studentDashboardViewModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();

		(BindingContext as StudentDashboardViewModel).GetCurrentStudentCommand.Execute(null);
	}
}