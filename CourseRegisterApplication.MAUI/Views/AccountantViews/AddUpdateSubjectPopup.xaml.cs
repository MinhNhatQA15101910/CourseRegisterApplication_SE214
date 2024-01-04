using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddUpdateSubjectPopup : Popup
{
    public AddUpdateSubjectPopup(AddUpdateSubjectViewModel addUpdateSubjectViewModel)
    {
        InitializeComponent();
        BindingContext = addUpdateSubjectViewModel;
    }
    private void ClosePopup(object sender, TappedEventArgs e)
    {
        Close();
    }
}