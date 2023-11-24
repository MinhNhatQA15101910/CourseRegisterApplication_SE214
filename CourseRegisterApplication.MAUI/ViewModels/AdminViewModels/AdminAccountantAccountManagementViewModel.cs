using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class UserDisplay : ObservableObject
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string roleName;

        [ObservableProperty]
        private Color roleBackgroundColor;

        [ObservableProperty]
        private Color backgroundColor;

        [ObservableProperty]
        private string avatar;

        public string PrimaryRoleName { get; set; }
        public IUserRequester UserRequester { get; set; }

        public ObservableCollection<string> Roles { get; set; } = new ObservableCollection<string>();

        public UserDisplay()
        {
            Roles = new ObservableCollection<string> { "Admin", "Accountant" };
        }

        [RelayCommand]
        public void ChooseAccount()
        {
            UserRequester.ResetItemBackgrounds();
            BackgroundColor = Color.FromArgb("#1E90FF");

            UserRequester.DisplayAccountInformation(new User
            {
                Username = Username,
                Email = Email,
                Role = (PrimaryRoleName == "Admin") ? Role.Admin : Role.Accountant
            });
        }

        partial void OnRoleNameChanged(string oldValue, string newValue)
        {
            if (newValue != PrimaryRoleName)
            {
                RoleBackgroundColor = Color.FromArgb("#ADD8E6");
            }
            else
            {
                RoleBackgroundColor = Color.FromArgb("#EBF6FF");
            }
        }
    }

    public interface IUserRequester
    {
        void ResetItemBackgrounds();
        void DisplayAccountInformation(User user);
    }

    public partial class AdminAccountantAccountManagementViewModel : ObservableObject, IUserRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        public ObservableCollection<UserDisplay> AdminAccountantAccountList { get; set; } = new ObservableCollection<UserDisplay>();
        public ObservableCollection<string> FilterOptions { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string avatarUri = "blank_avatar.jpg";

        [ObservableProperty]
        private string username = "";

        [ObservableProperty]
        private string roleName = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string selectedFilterOption = "Username";
        #endregion

        #region Constructor
        public AdminAccountantAccountManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userService = serviceProvider.GetService<IUserService>();

            FilterOptions = new ObservableCollection<string> { "Username", "Email", "Role" };
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
                        RoleName = (account.Role == Role.Admin) ? "Admin" : "Accountant",
                        PrimaryRoleName = (account.Role == Role.Admin) ? "Admin" : "Accountant",
                        Avatar = (account.Role == Role.Admin) ? "admin_avatar.jpg" : "accountant_avatar.png",
                        BackgroundColor = Color.FromArgb("#EBF6FF"),
                        RoleBackgroundColor = Color.FromArgb("#EBF6FF"),
                        UserRequester = this,
                    });
                }
            }
        }

        public void ResetItemBackgrounds()
        {
            foreach (var account in AdminAccountantAccountList)
            {
                account.BackgroundColor = Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayAccountInformation(User user)
        {
            AvatarUri = (user.Role == Role.Admin) ? "admin_avatar.jpg" : "accountant_avatar.png";
            Username = user.Username;
            Email = user.Email;
            RoleName = (user.Role == Role.Admin) ? "Admin" : "Accountant";
        }
    }
}
