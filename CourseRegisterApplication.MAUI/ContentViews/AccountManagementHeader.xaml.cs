using CourseRegisterApplication.MAUI.ContentViews;
using Mopups.Services;

namespace CourseRegisterApplication.MAUI.Views.ContentViews;

public partial class AccountManagementHeader : ContentView
{
    public static readonly BindableProperty CardDescriptionProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AccountManagementHeader), string.Empty);
    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(AccountManagementHeader), string.Empty);
    public static readonly BindableProperty SearchPlaceHolderProperty = BindableProperty.Create(nameof(SearchPlaceHolder), typeof(string), typeof(AccountManagementHeader), string.Empty);
    public static readonly BindableProperty IsBackVisibleProperty = BindableProperty.Create(nameof(IsBackVisible), typeof(bool), typeof(AccountManagementHeader), true);
    public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(nameof(IsTitleVisible), typeof(bool), typeof(AccountManagementHeader), true);
    public static readonly BindableProperty IsControlVisibleProperty = BindableProperty.Create(nameof(IsControlVisible), typeof(bool), typeof(AccountManagementHeader), true);


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
    public string SearchPlaceHolder
    {
        get => (string)GetValue(SearchPlaceHolderProperty);
        set => SetValue(SearchPlaceHolderProperty, value);
    }
    public AccountManagementHeader()
	{
		InitializeComponent();
	}

    [Obsolete]
    private void btnFilter_Clicked(object sender, EventArgs e)
    {
        btnFilter.BackgroundColor = Color.FromHex("#509CDB");
        MopupService.Instance.PushAsync(new FilterPopup());
    }
}