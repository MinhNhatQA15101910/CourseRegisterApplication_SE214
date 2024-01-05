using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class CurriculumManagementPage : ContentPage
{
	public CurriculumManagementPage(CurriculumManagementViewModel curriculumManagementViewModel)
	{
		InitializeComponent();
		BindingContext = curriculumManagementViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as CurriculumManagementViewModel).GetFilterOptionCommand.Execute(null);
	}
}