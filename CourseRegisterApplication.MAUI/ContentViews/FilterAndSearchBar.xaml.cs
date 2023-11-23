namespace CourseRegisterApplication.MAUI.ContentViews;

public partial class FilterAndSearchBar : ContentView
{
    #region Placeholder
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder),
            typeof(string),
            typeof(FilterAndSearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: TitleTextPropertyChanged);

    private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FilterAndSearchBar)bindable;
        control.SearchBar.Placeholder = (string)newValue;
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    #endregion

    public FilterAndSearchBar()
	{
		InitializeComponent();
	}
}