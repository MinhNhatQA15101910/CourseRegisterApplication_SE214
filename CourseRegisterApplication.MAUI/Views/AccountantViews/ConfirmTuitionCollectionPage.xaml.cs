using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class ConfirmTuitionCollectionPage : ContentPage
{
	public ConfirmTuitionCollectionPage(ConfirmTuitionCollectionViewModel confirmTuitionCollectionViewModel)
	{
		InitializeComponent();
		BindingContext = confirmTuitionCollectionViewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as ConfirmTuitionCollectionViewModel).GetTuitionFormListCommand.Execute(null);
    }
}