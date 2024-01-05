using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class UpdateCurriculumPage : ContentPage
{
	public UpdateCurriculumPage(UpdateCurriculumViewModel updateCurriculumViewModel)
	{
		InitializeComponent();
		BindingContext = updateCurriculumViewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as UpdateCurriculumViewModel).GetFilterOptionCommand.Execute(null);
    }
}