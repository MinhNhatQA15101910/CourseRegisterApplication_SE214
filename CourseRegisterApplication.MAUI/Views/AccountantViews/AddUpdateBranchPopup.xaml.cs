using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateBranchPopup : Popup
{
	public AddUpdateBranchPopup(AddUpdateBranchViewModel addUpdateBranchViewModel)
	{
		InitializeComponent();
        BindingContext = addUpdateBranchViewModel;
	}
}