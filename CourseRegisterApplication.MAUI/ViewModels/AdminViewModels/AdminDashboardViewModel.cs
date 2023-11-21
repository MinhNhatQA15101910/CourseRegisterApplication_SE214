using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AdminDashboardViewModel : ObservableObject
    {
        #region Services
        private IAlertService _alertService;
        private INavigationService _navigationService;
        #endregion

        #region Properties
        [ObservableProperty]
        private string descriptionText = FormatDateTime(DateTime.Now);
        #endregion

        public AdminDashboardViewModel(IServiceProvider serviceProvider)
        {
            _alertService = serviceProvider.GetService<IAlertService>();
            _navigationService = serviceProvider.GetService<INavigationService>();
        }

        [RelayCommand]
        public async Task Logout()
        {
            bool result = await _alertService.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (result)
            {
                await _navigationService.NavigateBackToRoot();
            }
        }

        private static string FormatDateTime(DateTime dateTime)
        {
            return "Today is " + dateTime.ToString("dddd, MMMM d\"th\", yyyy");
        }
    }
}
