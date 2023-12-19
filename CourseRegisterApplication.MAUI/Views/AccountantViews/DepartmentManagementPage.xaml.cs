using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class DepartmentManagementPage : ContentPage
{
	public DepartmentManagementPage(DepartmentManagementViewModel departmentManagementViewModel)
	{
		InitializeComponent();
		BindingContext = departmentManagementViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		(BindingContext as DepartmentManagementViewModel).GetDepartmentsCommand.Execute(null);
    }
}