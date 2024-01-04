using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

namespace CourseRegisterApplication.MAUI.Views.AccountantViews;

public partial class SubjectTypeManagementPage : ContentPage
{
	public SubjectTypeManagementPage(SubjectTypeManagementViewModel subjectTypeManagementViewModel)
	{
		InitializeComponent();
		BindingContext = subjectTypeManagementViewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        (BindingContext as SubjectTypeManagementViewModel).GetAllSubjectTypeCommand.Execute(null);
    }
}