using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateStudentPage : ContentPage
{
	public AddUpdateStudentPage(AddUpdateStudentViewModel addUpdateStudentViewModel)
	{
		InitializeComponent();
		BindingContext = addUpdateStudentViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as AddUpdateStudentViewModel).Clear();
    }
}