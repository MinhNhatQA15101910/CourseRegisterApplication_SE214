﻿using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    #region Displays
    public partial class UserDisplay : ObservableObject
    {
        #region Properties
        public int Id { get; set; }

        public string Password { get; set; }

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

        public ObservableCollection<string> Roles { get; set; } = new ObservableCollection<string> { "Admin", "Accountant" };
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseAccount()
        {
            UserRequester.ResetItemBackgrounds();
            BackgroundColor = Color.FromArgb("#1E90FF");

            UserRequester.DisplayAccountInformation(new User
            {
                Id = Id,
                Username = Username,
                Email = Email,
                Role = (PrimaryRoleName == "Admin") ? Role.Admin : Role.Accountant
            });
        }
        #endregion

        #region Property Changed
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

            if (UserRequester != null)
            {
                UserRequester.NotifyCanSaveChanges();
            }
        }
        #endregion
    }
    #endregion

    #region IRequesters
    public interface IUserRequester
    {
        void ResetItemBackgrounds();
        void DisplayAccountInformation(User user);
        void NotifyCanSaveChanges();
    }
    #endregion

    #region Main ViewModel
    public partial class AdminAccountantAccountManagementViewModel : ObservableObject, IUserRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        private List<UserDisplay> primaryAdminAccountantAccountList = new();

        [ObservableProperty]
        private ObservableCollection<UserDisplay> adminAccountantAccountList = new();

        public ObservableCollection<string> FilterOptions { get; set; } = new() { "Username", "Email", "Role" };

        public int Id { get; set; }

        [ObservableProperty]
        private string avatarUri = "blank_avatar.jpg";

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
        private string username = "";

        [ObservableProperty]
        private string roleName = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string selectedFilterOption = "Username";

        [ObservableProperty]
        private string searchFilter = "";
        #endregion

        #region Constructor
        public AdminAccountantAccountManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userService = serviceProvider.GetService<IUserService>();
        }
        #endregion

        #region Commands
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
            var accountList = await _userService.GetAdminAccountantUsers();
            ReloadAdminAccountantAccountList(accountList);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUser))]
        public async Task DeleteUser()
        {
            if (Username == GlobalConfig.CurrentUser.Username)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "You cannot delete your current login account!", "OK");
                return;
            }

            bool result = await Application.Current.MainPage.DisplayAlert("Warning", "Do you want to delete this account?", "Yes", "No");
            if (result)
            {
                bool deleteResult = await _userService.DeleteUser(Id);
                if (deleteResult)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "The user account has been deleted", "OK");
                    await GetAdminAccountantAccountCommand.ExecuteAsync(null);
                }
            }
        }

        public bool CanDeleteUser()
        {
            return Username != "";
        }

        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        public async Task SaveChanges()
        {
            // Filter accounts need to be updated
            List<User> users = await _userService.GetAdminAccountantUsers();
            List<User> updateUsers = users
                .Where(u => primaryAdminAccountantAccountList
                .Find(ud => ud.Username == u.Username && ud.RoleName != ud.PrimaryRoleName) != null)
                .ToList();

            // If there is current user account in the list, display error alert
            User currentUser = updateUsers.Find(a => a.Username == GlobalConfig.CurrentUser.Username);
            if (currentUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Cannot change Role of current user!", "OK");
                return;
            }

            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to change role for these accounts?", "Yes", "No");
            if (accept)
            {
                // Update each account's role
                bool result = true;
                foreach (var updateUser in updateUsers)
                {
                    Role updateRole = primaryAdminAccountantAccountList
                        .Find(u => u.Username == updateUser.Username).RoleName == "Admin" ? Role.Admin : Role.Accountant;

                    result = result && await _userService.UpdateRole(updateUser, updateRole);
                }

                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Changes saved!", "OK");
                    GetAdminAccountantAccountCommand.Execute(null);
                }
            }
        }

        public bool CanSaveChanges()
        {
            var account = primaryAdminAccountantAccountList.Find(a => a.RoleName != a.PrimaryRoleName);

            return (account != null);
        }

        [RelayCommand]
        public async Task NavigateToAddAdminAccountantAccountPage()
        {
            await Shell.Current.GoToAsync(nameof(AddAdminAccountantAccountPage), true);
        }
        #endregion

        #region Implement IUserRequester
        public void ResetItemBackgrounds()
        {
            foreach (var account in AdminAccountantAccountList)
            {
                account.BackgroundColor = Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayAccountInformation(User user)
        {
            Id = user.Id;
            AvatarUri = (user.Role == Role.Admin) ? "admin_avatar.jpg" : "accountant_avatar.png";
            Username = user.Username;
            Email = user.Email;
            RoleName = (user.Role == Role.Admin) ? "Admin" : "Accountant";
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                SearchFilter = "";
                ResetItemBackgrounds();
                ResetAccountInformation();

                switch (newValue)
                {
                    case "Username":
                        AdminAccountantAccountList = primaryAdminAccountantAccountList.OrderBy(a => a.Username).ToObservableCollection();
                        break;
                    case "Email":
                        AdminAccountantAccountList = primaryAdminAccountantAccountList.OrderBy(a => a.Email).ToObservableCollection();
                        break;
                    case "Role":
                        AdminAccountantAccountList = primaryAdminAccountantAccountList.OrderBy(a => a.RoleName).ToObservableCollection();
                        break;
                }
            }
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            ResetItemBackgrounds();
            ResetAccountInformation();

            switch (SelectedFilterOption)
            {
                case "Username":
                    AdminAccountantAccountList = primaryAdminAccountantAccountList.Where(a => a.Username.Contains(newValue)).OrderBy(a => a.Username).ToObservableCollection();
                    break;
                case "Email":
                    AdminAccountantAccountList = primaryAdminAccountantAccountList.Where(a => a.Email.Contains(newValue)).OrderBy(a => a.Email).ToObservableCollection();
                    break;
                case "Role":
                    AdminAccountantAccountList = primaryAdminAccountantAccountList.Where(a => a.RoleName.Contains(newValue)).OrderBy(a => a.RoleName).ToObservableCollection();
                    break;
            }
        }
        #endregion

        #region Helpers
        private void ReloadAdminAccountantAccountList(List<User> accountList)
        {
            primaryAdminAccountantAccountList.Clear();

            if (accountList.Count > 0)
            {
                foreach (var account in accountList)
                {
                    primaryAdminAccountantAccountList.Add(new UserDisplay
                    {
                        Id = account.Id,
                        Username = account.Username,
                        Password = account.Password,
                        Email = account.Email,
                        RoleName = (account.Role == Role.Admin) ? "Admin" : "Accountant",
                        PrimaryRoleName = (account.Role == Role.Admin) ? "Admin" : "Accountant",
                        Avatar = (account.Role == Role.Admin) ? "admin_avatar.jpg" : "accountant_avatar.png",
                        BackgroundColor = Color.FromArgb("#EBF6FF"),
                        RoleBackgroundColor = Color.FromArgb("#EBF6FF"),
                        UserRequester = this,
                    });
                }

                AdminAccountantAccountList = primaryAdminAccountantAccountList.OrderBy(a => a.Username).ToObservableCollection();
            }
        }

        private void ResetAccountInformation()
        {
            Id = 0;
            AvatarUri = "blank_avatar.jpg";
            Username = "";
            Email = "";
            RoleName = "";
        }

        public void NotifyCanSaveChanges()
        {
            SaveChangesCommand.NotifyCanExecuteChanged();
        }
        #endregion
    }
    #endregion
}
