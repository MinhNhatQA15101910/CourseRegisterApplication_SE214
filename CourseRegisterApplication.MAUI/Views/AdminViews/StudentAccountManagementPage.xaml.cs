using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class StudentAccountManagementPage : ContentPage
{
	public StudentAccountManagementPage(StudentAccountManagementViewModel studentAccountManagementViewModel)
	{
		InitializeComponent();
		BindingContext = studentAccountManagementViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as StudentAccountManagementViewModel).ResetAccountInformation();
        (BindingContext as StudentAccountManagementViewModel).GetStudentAccountsCommand.Execute(null);
    }
}