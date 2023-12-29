using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class SubjectManagementPage : ContentPage
{
	public SubjectManagementPage(SubjectManagementViewModel subjectManagementViewModel)
	{
		InitializeComponent();
		BindingContext = subjectManagementViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }
}