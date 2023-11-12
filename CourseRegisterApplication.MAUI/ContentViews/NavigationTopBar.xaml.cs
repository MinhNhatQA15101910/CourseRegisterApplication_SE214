using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.Views;
using Microsoft.Maui;
namespace CourseRegisterApplication.MAUI.ContentViews;

public partial class NavigationTopBar : ContentView
{
    #region TitleText
    public static readonly BindableProperty TitleTextProperty = 
        BindableProperty.Create(nameof(TitleText), 
            typeof(string), 
            typeof(NavigationTopBar), 
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: TitleTextPropertyChanged);

    private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (NavigationTopBar)bindable;
        control.Title.Text = (string)newValue;
    }

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }
    #endregion

    #region DescriptionText
    public static readonly BindableProperty DescriptionTextProperty =
        BindableProperty.Create(nameof(DescriptionText),
            typeof(string),
            typeof(NavigationTopBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: DescriptionTextPropertyChanged);

    private static void DescriptionTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (NavigationTopBar)bindable;
        control.Description.Text = (string)newValue;
    }
    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }
    #endregion

    #region IsBackVisible
    public static readonly BindableProperty IsBackVisibleProperty 
        = BindableProperty.Create(nameof(IsBackVisible), 
            typeof(bool), 
            typeof(NavigationTopBar), 
            defaultValue: true,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: IsBackVisiblePropertyChanged);

    private static void IsBackVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (NavigationTopBar)bindable;
        control.NavigateBackGroup.IsVisible = (bool)newValue;
    }

	public bool IsBackVisible
	{
		get => (bool)GetValue(IsBackVisibleProperty);
		set => SetValue(IsBackVisibleProperty, value);
	}
    #endregion

    #region IsTitleVisible
    public static readonly BindableProperty IsTitleVisibleProperty 
        = BindableProperty.Create(nameof(IsTitleVisible), 
            typeof(bool), 
            typeof(NavigationTopBar), 
            defaultValue: true,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: IsTitleVisiblePropertyChanged);

    private static void IsTitleVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (NavigationTopBar)bindable;
        control.TitleGroup.IsVisible = (bool)newValue;
    }

	public bool IsTitleVisible
	{
		get => (bool)GetValue(IsTitleVisibleProperty);
		set => SetValue(IsTitleVisibleProperty, value);
	}
    #endregion

    #region IsControlVisible
    public static readonly BindableProperty IsControlVisibleProperty 
        = BindableProperty.Create(nameof(IsControlVisible), 
            typeof(bool), 
            typeof(NavigationTopBar), 
            defaultValue: true,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: IsControlVisiblePropertyChanged);

    private static void IsControlVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (NavigationTopBar)bindable;
        control.Control.IsVisible = (bool)newValue;
    }

	public bool IsControlVisible
	{
		get => (bool)GetValue(IsControlVisibleProperty);
		set => SetValue(IsControlVisibleProperty, value);
	}
    #endregion

    #region LogoutCommand
    public static readonly BindableProperty LogoutCommandProperty 
        = BindableProperty.Create(nameof(LogoutCommand), 
            typeof(ICommand), 
            typeof(NavigationTopBar), 
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: LogoutCommandPropertyChanged);

    private static void LogoutCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
        var control = (NavigationTopBar)bindable;
        control.LogoutButton.Command = (ICommand)newValue;
	}

    public ICommand LogoutCommand
	{
        get => (ICommand)GetValue(LogoutCommandProperty);
        set => SetValue(LogoutCommandProperty, value);
	}
    #endregion

    #region NavigateBackCommand
    public static readonly BindableProperty NavigateBackCommandProperty
        = BindableProperty.Create(nameof(NavigateBackCommand),
            typeof(ICommand),
            typeof(NavigationTopBar),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: NavigateBackCommandPropertyChanged);

    private static void NavigateBackCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
        var control = (NavigationTopBar)bindable;
        control.OnTapBackImage.Command = (ICommand)newValue;
        control.OnTapBackLabel.Command = (ICommand)newValue;
	}

    public ICommand NavigateBackCommand
	{
        get => (ICommand)GetValue(NavigateBackCommandProperty);
        set => SetValue(NavigateBackCommandProperty, value);
	}
    #endregion

    #region Constructor
    public NavigationTopBar()
	{
        InitializeComponent();
	}
    #endregion
}