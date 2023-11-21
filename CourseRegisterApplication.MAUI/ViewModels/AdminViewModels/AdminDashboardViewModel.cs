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
            int day = dateTime.Day;

            if (day == 1 || day == 21 || day == 31)
            {
                return "Today is " + dateTime.ToString("dddd, MMMM d\"st\", yyyy");
            }

            if (day == 2 || day == 22)
            {
                return "Today is " + dateTime.ToString("dddd, MMMM d\"nd\", yyyy");
            }

            if (day == 3 || day == 23)
            {
                return "Today is " + dateTime.ToString("dddd, MMMM d\"rd\", yyyy");
            }

            return "Today is " + dateTime.ToString("dddd, MMMM d\"th\", yyyy");
        }
    }
}
