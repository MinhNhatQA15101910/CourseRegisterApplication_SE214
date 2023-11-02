using System.ComponentModel;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public class MenuItems : INotifyPropertyChanged
	{
		private Color color;
		public string Title { get; set; }
		public string Icon { get; set; }

		public Color Color
		{
			get { return color; }
			set
			{
				if (color != value)
				{
					color = value;
					OnPropertyChanged("Color");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
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
