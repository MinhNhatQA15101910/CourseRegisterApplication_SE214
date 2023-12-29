using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class AddSubjectPopup : Popup
{
    public AddSubjectPopup(AddSubjectViewModel addSubjectViewModel)
    {
        InitializeComponent();
        BindingContext = addSubjectViewModel;
    }
    private void ClosePopup(object sender, TappedEventArgs e)
    {
        Close();
    }
}