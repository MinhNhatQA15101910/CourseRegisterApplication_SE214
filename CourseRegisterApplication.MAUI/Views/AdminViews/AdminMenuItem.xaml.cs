using CourseRegisterApplication_SE214.ViewModels.AdminViewModels;

namespace CourseRegisterApplication_SE214.Views.AdminViews;

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
				selectedItem.Color = Color.FromHex("#152259");
				OnPropertyChanged("Color");
			}

			newSelectedItem.Color = Color.FromHex("#509CDB");
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