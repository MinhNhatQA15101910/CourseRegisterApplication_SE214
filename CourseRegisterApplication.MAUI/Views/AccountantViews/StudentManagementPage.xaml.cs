using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class StudentManagementPage : ContentPage
{
	public StudentManagementPage(StudentManagementViewModel studentManagementViewModel)
	{
		InitializeComponent();
		BindingContext = studentManagementViewModel;
	}
	protected override void OnAppearing()
	{
        base.OnAppearing();

        (BindingContext as StudentManagementViewModel).GetStudentAccountCommand.Execute(null);
    }
}