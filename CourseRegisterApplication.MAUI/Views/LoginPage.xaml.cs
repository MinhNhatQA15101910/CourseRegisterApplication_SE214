using CourseRegisterApplication_SE214.ViewModels;
namespace CourseRegisterApplication_SE214.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel(this);
	}
}