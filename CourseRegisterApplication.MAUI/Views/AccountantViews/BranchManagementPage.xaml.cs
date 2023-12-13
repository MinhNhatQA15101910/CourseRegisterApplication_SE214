using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class BranchManagementPage : ContentPage
{
    public BranchManagementPage(BranchManagementViewModel branchManagementViewModel)
    {
        InitializeComponent();
        BindingContext = branchManagementViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        (BindingContext as BranchManagementViewModel).GetBranchesCommand.Execute(null);
    }
}