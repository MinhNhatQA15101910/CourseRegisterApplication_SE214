using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.Views;
using Microsoft.Maui;
namespace CourseRegisterApplication.MAUI.ContentViews;

public partial class NavigationTopBar : ContentView
{
	public static readonly BindableProperty CardDescriptionProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationTopBar), string.Empty);
	public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(NavigationTopBar), string.Empty);
	public static readonly BindableProperty IsBackVisibleProperty = BindableProperty.Create(nameof(IsBackVisible), typeof(bool), typeof(NavigationTopBar), true);
	public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(nameof(IsTitleVisible), typeof(bool), typeof(NavigationTopBar), true);
	public static readonly BindableProperty IsControlVisibleProperty = BindableProperty.Create(nameof(IsControlVisible), typeof(bool), typeof(NavigationTopBar), true);



	public bool IsBackVisible
	{
		get => (bool)GetValue(IsBackVisibleProperty);
		set => SetValue(IsBackVisibleProperty, value);
	}
	public bool IsTitleVisible
	{
		get => (bool)GetValue(IsTitleVisibleProperty);
		set => SetValue(IsTitleVisibleProperty, value);
	}
	public bool IsControlVisible
	{
		get => (bool)GetValue(IsControlVisibleProperty);
		set => SetValue(IsControlVisibleProperty, value);
	}
	public string Title
	{
		get => (string)GetValue(CardDescriptionProperty);
		set => SetValue(CardDescriptionProperty, value);
	}
	public string Description
	{
		get => (string)GetValue(DescriptionProperty);
		set => SetValue(DescriptionProperty, value);
	}
	public NavigationTopBar()
	{
		InitializeComponent();
	}

	private void OnChangePasswordTapped(object sender, EventArgs e)
	{
		App.Current.MainPage=new ChangePasswordPage();
	}

	private void OnLogoutTapped(object sender, EventArgs e)
	{
		App.Current.MainPage = new LoginPage();
	}
}