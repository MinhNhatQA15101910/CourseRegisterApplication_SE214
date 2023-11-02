using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

namespace CourseRegisterApplication.MAUI.Views.AdminViews;

public partial class AdminMenuItem : ContentPage
{
	public event EventHandler<Type> PageChanged;
	public AdminMenuItem()
	{
		InitializeComponent();
		BindingContext = new AdminFlyoutItem();
	}
	private MenuItems selectedItem;

	private void OnItemTapped(object sender, ItemTappedEventArgs e)
	{
		if (e.Item is MenuItems newSelectedItem)
		{
			Type pageType = null;

			if (selectedItem != null)
			{
				selectedItem.Color = Color.FromArgb("#152259");
				OnPropertyChanged("Color");
			}

			newSelectedItem.Color = Color.FromArgb("#509CDB");
			OnPropertyChanged("Color");

			selectedItem = newSelectedItem;

			if (newSelectedItem.Title == "Dashboard")
			{
				pageType = typeof(AdminDashboardPage);
			}
			else if (newSelectedItem.Title == "Accountant account")
			{
				pageType = typeof(EmployeeAccountManagementPage);
			}
			else if (newSelectedItem.Title == "Student account")
			{
				pageType = typeof(StudentAccountManagementPage);
			}
			else if (newSelectedItem.Title == "Admin account")
			{
				pageType = typeof(AdminAccountManagementPage);
			}
			PageChanged?.Invoke(this, pageType);
		}
	}

}