using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class UnpayTuitionStudentsReportPage : ContentPage
{
	public UnpayTuitionStudentsReportPage(UnpayTuitionStudentsReportViewModel unpayTuitionStudentsReportViewModel)
	{
		InitializeComponent();
		BindingContext = unpayTuitionStudentsReportViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as UnpayTuitionStudentsReportViewModel).GetSetUpCommand.Execute(null);
    }
}