using CourseRegisterApplication.MAUI.ViewModels.StudentViewModels;

namespace CourseRegisterApplication.MAUI.Views.StudentViews;

public partial class PaymentPopup : Popup
{
	public PaymentPopup(TuitionInfoViewModel tuitionInfoViewModel)
	{
		InitializeComponent();
		BindingContext = tuitionInfoViewModel;
    }

}