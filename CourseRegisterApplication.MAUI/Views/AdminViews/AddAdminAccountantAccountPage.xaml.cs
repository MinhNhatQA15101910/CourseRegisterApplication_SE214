using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AddAdminAccountantAccountPage : ContentPage
{
	public AddAdminAccountantAccountPage(AddAdminAccountantAccountViewModel addAdminAccountantAccountViewModel)
	{
		InitializeComponent();
		BindingContext = addAdminAccountantAccountViewModel;
	}

	private void OnEyeTapped(object sender, EventArgs e)
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

    private void OnEyeTapped2(object sender, EventArgs e)
    {
        string closeIconPath = "eye_close_icon.png";
        string openIconPath = "eye_open_icon.png";

        if (eye_icon_2.Source is FileImageSource fileImageSource && fileImageSource.File == closeIconPath)
        {
            eye_icon_2.Source = openIconPath;
            confirmPassword.IsPassword = false;
		}
		else
        {
            eye_icon_2.Source = closeIconPath;
            confirmPassword.IsPassword = true;
        }
    }
}