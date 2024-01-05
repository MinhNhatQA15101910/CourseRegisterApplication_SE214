using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;
namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class TuitionInfoPage : ContentPage
{
	public TuitionInfoPage(TuitionInfoViewModel tuitionInfoViewModel)
	{
		InitializeComponent();
        BindingContext = tuitionInfoViewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as TuitionInfoViewModel).GetSetUpCommand.Execute(null);
    }
}