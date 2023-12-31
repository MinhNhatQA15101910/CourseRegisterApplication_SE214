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
    public void ShowPopup()
    {
        // Code to show the popup goes here

        // After the popup is displayed, execute the command
        (BindingContext as AddSubjectViewModel).GetInformationCommand.Execute(null);
    }
}