using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;

namespace CourseRegisterApplication.MAUI.Views.StudentViews;

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

		(BindingContext as StudentDashboardViewModel).InitialCommand.Execute(null);
	}
}