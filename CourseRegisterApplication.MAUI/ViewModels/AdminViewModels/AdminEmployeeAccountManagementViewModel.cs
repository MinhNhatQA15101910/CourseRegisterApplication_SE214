using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AdminEmployeeAccountManagementViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        public AdminEmployeeAccountManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        public async Task Logout()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (result)
            {
                Application.Current.MainPage = _serviceProvider.GetService<LoginPage>();
            }
        }
    }
}
