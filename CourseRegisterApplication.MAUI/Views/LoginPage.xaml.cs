using CourseRegisterApplication.MAUI.IViewModels;
using CourseRegisterApplication.MAUI.IViews;
namespace CourseRegisterApplication.MAUI.Views;

public partial class LoginPage : ContentPage, ILoginPage
{
	public LoginPage(ILoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel;
	}

    public void OnEyeTapped(object sender, EventArgs e)
	{
		string closeIconPath = "eye_close_icon.png";
		string openIconPath = "eye_open_icon.png";

		if (eye_icon.Source is FileImageSource fileImageSource && fileImageSource.File == closeIconPath)
		{
			eye_icon.Source = openIconPath;
			password.IsPassword = false;
		}
		else
		{
			eye_icon.Source = closeIconPath;
			password.IsPassword = true;
		}
	}
}