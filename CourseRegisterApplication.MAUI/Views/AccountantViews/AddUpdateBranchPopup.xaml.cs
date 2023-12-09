using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateBranchPopup : Popup
{
	public AddUpdateBranchPopup(AddUpdateBranchViewModel addUpdateBranchViewModel)
	{
		InitializeComponent();

        addUpdateBranchViewModel.GetDepartmentsCommand.Execute(null);
        BindingContext = addUpdateBranchViewModel;
	}
}