using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class ProvinceDistrictManagementPage : ContentPage
{
	public ProvinceDistrictManagementPage(ProvinceDistrictManagementViewModel provinceDistrictManagementViewModel)
	{
		InitializeComponent();
		BindingContext = provinceDistrictManagementViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as ProvinceDistrictManagementViewModel).GetProvincesAndDistrictsCommand.Execute(null);
    }
}