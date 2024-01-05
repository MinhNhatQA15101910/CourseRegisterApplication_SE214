using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

public partial class FilterStudentAccountViewModel : ObservableObject
{
    #region Services
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region Properties
    [ObservableProperty] private List<string> filterActiveStatusOptions = new() { "All", "Enable", "Disable" };

    [ObservableProperty] private string selectedActiveStatus = "All";

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool studentNameAZChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool studentNameZAChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool studentIdAZChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool studentIdZAChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool emailAZChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool emailZAChecked;

    #endregion

    #region Constructor
    public FilterStudentAccountViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ClosePopup()
    {
        Popup popup = _serviceProvider.GetService<FilterStudentAccountPopup>();
        await popup.CloseAsync();
    }

    [RelayCommand(CanExecute = nameof(CanConfirmSortCommandExecuted))]
    public async Task ConfirmSort()
    {
        StudentAccountManagementViewModel studentAccountManagementViewModel = _serviceProvider.GetService<StudentAccountManagementViewModel>();

        if (StudentNameAZChecked)
        {
            studentAccountManagementViewModel.UpdateSortList(StudentAccountSortType.StudentNameAZ, SelectedActiveStatus);
        }
        else if (StudentNameZAChecked)
        {
            studentAccountManagementViewModel.UpdateSortList(StudentAccountSortType.StudentNameZA, SelectedActiveStatus);
        }
        else if (StudentIdAZChecked)
        {
            studentAccountManagementViewModel.UpdateSortList(StudentAccountSortType.StudentIdAZ, SelectedActiveStatus);
        }
        else if (StudentIdZAChecked)
        {
            studentAccountManagementViewModel.UpdateSortList(StudentAccountSortType.StudentIdZA, SelectedActiveStatus);
        }
        else if (EmailAZChecked)
        {
            studentAccountManagementViewModel.UpdateSortList(StudentAccountSortType.EmailAZ, SelectedActiveStatus);
        }
        else if (EmailZAChecked)
        {
            studentAccountManagementViewModel.UpdateSortList(StudentAccountSortType.EmailAZ, SelectedActiveStatus);
        }

        await ClosePopup();
    }

    private bool CanConfirmSortCommandExecuted()
    {
        return StudentIdAZChecked || StudentIdZAChecked
            || StudentNameAZChecked || StudentNameZAChecked
            || EmailAZChecked || EmailZAChecked;
    }
    #endregion
}
