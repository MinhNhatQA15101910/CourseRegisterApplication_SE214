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
        //ManagerAccountManagementViewModel managerAccountManagementViewModel = _serviceProvider.GetService<ManagerAccountManagementViewModel>();

        //if (UsernameAZChecked)
        //{
        //    managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.UsernameAZ, SelectedFilterAccountType);
        //}
        //else if (UsernameZAChecked)
        //{
        //    managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.UsernameZA, SelectedFilterAccountType);
        //}
        //else if (EmailAZChecked)
        //{
        //    managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.EmailAZ, SelectedFilterAccountType);
        //}
        //else if (EmailZAChecked)
        //{
        //    managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.EmailAZ, SelectedFilterAccountType);
        //}
        //else if (RoleAZChecked)
        //{
        //    managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.RoleAZ, SelectedFilterAccountType);
        //}
        //else if (RoleZAChecked)
        //{
        //    managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.RoleZA, SelectedFilterAccountType);
        //}

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
