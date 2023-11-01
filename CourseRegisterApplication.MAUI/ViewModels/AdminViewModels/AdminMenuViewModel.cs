using System.Collections.ObjectModel;
namespace CourseRegisterApplication_SE214.ViewModels.AdminViewModels
{
	public class MenuItems
	{
		public string Title { get; set; }
		public string Icon { get; set; }
		public Color Color { get; set; }
	}

	public class AdminFlyoutItem
	{
		public ObservableCollection<MenuItems> AdminFlyoutItems { get; } = new ObservableCollection<MenuItems>();
		public AdminFlyoutItem()
		{
			AdminFlyoutItems.Add(new MenuItems { Title = "Dashboard", Icon = "home.png", Color = Color.FromArgb("#152259") });
			AdminFlyoutItems.Add(new MenuItems { Title = "Accountant account", Icon = "student.png", Color = Color.FromArgb("#152259") });
			AdminFlyoutItems.Add(new MenuItems { Title = "Student account", Icon = "student.png", Color = Color.FromArgb("#152259") });
			AdminFlyoutItems.Add(new MenuItems { Title = "Admin account", Icon = "book.png", Color = Color.FromArgb("#152259") });
		}

	}
}
