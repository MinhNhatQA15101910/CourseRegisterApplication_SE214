namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminFlyoutPage : FlyoutPage
{
	public AdminFlyoutPage()
	{
		InitializeComponent();
		Detail = new NavigationPage(new AdminDashboardPage());
		adminFlyoutPage.PageChanged += OnPageChanged;
	}
	private void OnPageChanged(object sender, Type pageType)
	{
		if (pageType != null)
		{
			Detail = new NavigationPage((Page)Activator.CreateInstance(pageType));
		}
	}
}