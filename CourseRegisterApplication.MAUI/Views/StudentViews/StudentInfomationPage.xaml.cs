using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;

namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class StudentInfomationPage : ContentPage
{
	public StudentInfomationPage(StudentInfomationViewModel studentInfomationViewModel)
	{
		InitializeComponent();
		BindingContext = studentInfomationViewModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();

		(BindingContext as StudentInfomationViewModel).GetCurrentStudentInfomationCommand.Execute(null);
	}
}