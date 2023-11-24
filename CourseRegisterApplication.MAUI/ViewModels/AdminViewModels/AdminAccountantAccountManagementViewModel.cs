using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AdminAccountantAccountManagementViewModel : ObservableObject
    {
        public class RoleDisplay
        {
            public int Id { get; set; }
            public string RoleName { get; set; }
        }

        public partial class UserDisplay : ObservableObject
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public RoleDisplay Role { get; set; }
            public ObservableCollection<RoleDisplay> RoleNames { get; set; } = new ObservableCollection<RoleDisplay>
            {
                new() {Id = 1, RoleName = "Admin"},
                new() {Id = 2, RoleName = "Accountant"}
            };
        }

        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        public ObservableCollection<UserDisplay> AdminAccountantAccountList { get; set; } = new ObservableCollection<UserDisplay>();
        public ObservableCollection<string> RoleNames { get; set; } = new ObservableCollection<string> { "Admin", "Accountant" };

        [ObservableProperty]
        private string roleName;
        #endregion

        #region Constructor
        public AdminAccountantAccountManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userService = serviceProvider.GetService<IUserService>();
        }
        #endregion

        [RelayCommand]
        public async Task Logout()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (result)
            {
                Application.Current.MainPage = _serviceProvider.GetService<LoginPage>();
            }
        }

        [RelayCommand]
        public async Task GetAdminAccountantAccount()
        {
            var accounts = await _userService.GetAdminAccountantAccounts();
            if (accounts?.Count > 0) 
            {
                AdminAccountantAccountList.Clear();
                foreach (var account in accounts)
                {
                    AdminAccountantAccountList.Add(new UserDisplay
                    {
                        Username = account.Username,
                        Email = account.Email,
                        Role = (account.Role == Role.Admin) ? new() { Id = 1, RoleName = "Admin" } : new() { Id = 2, RoleName = "Accountant" }
                    });
                }
            }
        }
    }
}
