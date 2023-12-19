using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AdminDashboardViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty]
        private string descriptionText = Helpers.FormatDateTime(DateTime.Now);
        #endregion

        public AdminDashboardViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        public async Task Logout()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (result)
            {
                var loginPage = _serviceProvider.GetService<LoginPage>();
                Application.Current.MainPage = loginPage;
            }
        }

        [RelayCommand]
        public async Task NavigateToAdminAccountantAccountManagement()
        {
            await Shell.Current.GoToAsync("//adminaccountant/details", true);
        }

        [RelayCommand]
        public async Task NavigateToStudentAccountManagement()
        {
            await Shell.Current.GoToAsync("//student/details", true);
        }
    }
}
