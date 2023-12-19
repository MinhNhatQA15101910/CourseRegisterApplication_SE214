using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddStudentPage : ContentPage
{
	public AddStudentPage(AddStudentViewModel addStudentViewModel)
	{
		InitializeComponent();
		BindingContext = addStudentViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as AddStudentViewModel).GetInformationCommand.Execute(null);
    }

}