using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;

namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class StudentAppShell : Shell
{
	public StudentAppShell(StudentAppShellViewModel studentAppShellViewModel)
	{
		InitializeComponent();
		BindingContext = studentAppShellViewModel;
    }
}