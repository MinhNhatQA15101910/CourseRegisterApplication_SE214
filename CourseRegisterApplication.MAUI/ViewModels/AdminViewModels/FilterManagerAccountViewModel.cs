using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

public partial class FilterManagerAccountViewModel : ObservableObject
{
    #region Services
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region Properties
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))] 
    private bool usernameAZChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool usernameZAChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))]
    private bool emailAZChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))] 
    private bool emailZAChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))] 
    private bool roleAZChecked;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ConfirmSortCommand))] 
    private bool roleZAChecked;

    [ObservableProperty] private List<string> filterAccountTypeOptions = new() { "All", "Admin", "Accountant" };

    [ObservableProperty] private string selectedFilterAccountType = "All";
    #endregion

    #region Constructor
    public FilterManagerAccountViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ClosePopup()
    {
        Popup popup = _serviceProvider.GetService<FilterManagerAccountPopup>();
        await popup.CloseAsync();
    }

    [RelayCommand(CanExecute = nameof(CanConfirmSortCommandExecuted))]
    public async Task ConfirmSort()
    {
        ManagerAccountManagementViewModel managerAccountManagementViewModel = _serviceProvider.GetService<ManagerAccountManagementViewModel>();

        if (UsernameAZChecked)
        {
            managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.UsernameAZ, SelectedFilterAccountType);
        }
        else if (UsernameZAChecked)
        {
            managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.UsernameZA, SelectedFilterAccountType);
        }
        else if (EmailAZChecked)
        {
            managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.EmailAZ, SelectedFilterAccountType);
        }
        else if (EmailZAChecked)
        {
            managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.EmailAZ, SelectedFilterAccountType);
        }
        else if (RoleAZChecked)
        {
            managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.RoleAZ, SelectedFilterAccountType);
        }
        else if (RoleZAChecked)
        {
            managerAccountManagementViewModel.UpdateSortList(ManagerAccountSortType.RoleZA, SelectedFilterAccountType);
        }

        await ClosePopup();
    }

    private bool CanConfirmSortCommandExecuted()
    {
        return UsernameAZChecked || UsernameZAChecked
            || EmailAZChecked || EmailZAChecked
            || RoleAZChecked || RoleZAChecked;
    }
    #endregion
}
