using CourseRegisterApplication.MAUI.ViewModels;
namespace CourseRegisterApplication.MAUI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel(this);
	}
}