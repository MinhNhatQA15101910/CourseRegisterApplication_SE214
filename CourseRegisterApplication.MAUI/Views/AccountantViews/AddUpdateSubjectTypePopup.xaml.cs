using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateSubjectTypePopup : Popup
{
	public AddUpdateSubjectTypePopup(AddUpdateSubjectTypeViewModel addUpdateSubjectTypeViewModel)
	{
        InitializeComponent();
		BindingContext = addUpdateSubjectTypeViewModel;
	}
}