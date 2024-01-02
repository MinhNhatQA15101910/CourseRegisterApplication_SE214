using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;
namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class CourseRegistrationInfoPage : ContentPage
{
	public CourseRegistrationInfoPage(CourseRegisTrationInfoViewModel courseRegisTrationInfoViewModel)
	{
		InitializeComponent();
        BindingContext = courseRegisTrationInfoViewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as CourseRegisTrationInfoViewModel).GetSetUpCommand.Execute(null);
    }
}