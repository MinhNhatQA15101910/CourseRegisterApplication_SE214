using CourseRegisterApplication.MAUI.ViewModels;
namespace CourseRegisterApplication.MAUI.Views;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage()
	{
		InitializeComponent();
		BindingContext = new ChangePasswordViewModel(this);
	}
	private void OnEyeTapped1(object sender, EventArgs e)
	{
		string closeIconPath = "eye_close_icon.png";
		string openIconPath = "eye_open_icon.png";

		if (eye_icon_1.Source is FileImageSource fileImageSource && fileImageSource.File == closeIconPath)
		{
			eye_icon_1.Source = openIconPath;
			password1.IsPassword = false;
		}
		else
		{
			eye_icon_1.Source = closeIconPath;
			password1.IsPassword = true;
		}
	}
	private void OnEyeTapped2(object sender, EventArgs e)
	{
		string closeIconPath = "eye_close_icon.png";
		string openIconPath = "eye_open_icon.png";

		if (eye_icon_2.Source is FileImageSource fileImageSource && fileImageSource.File == closeIconPath)
		{
			eye_icon_2.Source = openIconPath;
			password2.IsPassword = false;
		}
		else
		{
			eye_icon_2.Source = closeIconPath;
			password2.IsPassword = true;
		}
	}
	private void OnEyeTapped3(object sender, EventArgs e)
	{
		string closeIconPath = "eye_close_icon.png";
		string openIconPath = "eye_open_icon.png";

		if (eye_icon_3.Source is FileImageSource fileImageSource && fileImageSource.File == closeIconPath)
		{
			eye_icon_3.Source = openIconPath;
			password3.IsPassword = false;
		}
		else
		{
			eye_icon_3.Source = closeIconPath;
			password3.IsPassword = true;
		}
	}
}