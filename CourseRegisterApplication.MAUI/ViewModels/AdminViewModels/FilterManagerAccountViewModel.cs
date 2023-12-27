using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;

public partial class FilterManagerAccountViewModel : ObservableObject
{
    #region Services
    private readonly IServiceProvider _serviceProvider;
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
        ClearState();

        Popup popup = _serviceProvider.GetService<FilterManagerAccountPopup>();
        await popup.CloseAsync();
    }
    #endregion

    #region Helpers
    private void ClearState()
    {

    }
    #endregion
}
