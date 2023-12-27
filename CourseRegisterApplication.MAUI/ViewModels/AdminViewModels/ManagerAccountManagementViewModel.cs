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

        public int Index { get; set; }

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
		private bool isVisibleDot;

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
            BackgroundColor = Color.FromArgb("#B9D8F2");

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
                IsVisibleDot = true; 
            }
            else
            {
				IsVisibleDot = false;
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
    public partial class ManagerAccountManagementViewModel : ObservableObject, IUserRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        private readonly List<UserDisplay> primaryManagerAccountList = new();

        [ObservableProperty]
        private ObservableCollection<UserDisplay> managerAccountList = new();

        public ObservableCollection<string> FilterAccountTypeOptions { get; set; } = new() { "All", "Admin", "Accountant" };

        public int Id { get; set; } = -1;

        [ObservableProperty]
        private string avatarUri = "blank_avatar.jpg";

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
        private string username = "";

        [ObservableProperty]
        private string roleName = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string selectedFilterAccountTypeOptions = "All";

        [ObservableProperty]
        private string searchFilter = "";

		[ObservableProperty]
		private bool usernameAZChecked = true;
		[ObservableProperty]
		private bool usernameZAChecked = false;
		[ObservableProperty]
		private bool emailAZChecked = false;
		[ObservableProperty]
		private bool emailZAChecked = false;
		[ObservableProperty]
		private bool roleAZChecked = false;
		[ObservableProperty]
		private bool roleZAChecked = false;
		#endregion

		#region Constructor
		public ManagerAccountManagementViewModel(IServiceProvider serviceProvider)
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
        public async Task GetManagerAccount()
        {
            var accountList = await _userService.GetManagerUsers();
            ReloadManagerAccountList(accountList);
        }

        [RelayCommand]
        public async Task OpenFilterPopup()
        {
            var filterManagerAccountPopup = _serviceProvider.GetService<FilterManagerAccountPopup>();
            await Application.Current.MainPage.ShowPopupAsync(filterManagerAccountPopup);
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
                    await GetManagerAccountCommand.ExecuteAsync(null);
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
            List<User> users = await _userService.GetManagerUsers();
            List<User> updateUsers = users
                .Where(u => primaryManagerAccountList
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
                    Role updateRole = primaryManagerAccountList
                        .Find(u => u.Username == updateUser.Username).RoleName == "Admin" ? Role.Admin : Role.Accountant;

                    result = result && await _userService.UpdateRole(updateUser, updateRole);
                }

                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Changes saved!", "OK");
                    GetManagerAccountCommand.Execute(null);
                }
            }
        }

        public bool CanSaveChanges()
        {
            var account = primaryManagerAccountList.Find(a => a.RoleName != a.PrimaryRoleName);

            return (account != null);
        }

        [RelayCommand]
        public async Task NavigateToAddManagerAccountPage()
        {
            await Shell.Current.GoToAsync(nameof(AddManagerAccountPage), true);
        }
        #endregion

        #region Implement IUserRequester
        public void ResetItemBackgrounds()
        {
            for (int i = 0; i < ManagerAccountList.Count; i++)
            {
                ManagerAccountList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
			}
        }

        public void DisplayAccountInformation(User user)
        {
            Id = user.Id;
            AvatarUri = (user.Role == Role.Admin) ? "admin_icon.png" : "teacher_icon.png";
            Username = user.Username;
            Email = user.Email;
            RoleName = (user.Role == Role.Admin) ? "Admin" : "Accountant";
        }
        #endregion

        #region Property Changed
        public void UpdateSortList()
        {
            if(UsernameAZChecked)
            {
				ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.Username).ToObservableCollection();
			}
			if (UsernameZAChecked)
			{
				ManagerAccountList = primaryManagerAccountList.OrderByDescending(a => a.Username).ToObservableCollection();
			}
			if (EmailAZChecked)
			{
				ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.Email).ToObservableCollection();
			}
			if (EmailZAChecked)
			{
				ManagerAccountList = primaryManagerAccountList.OrderByDescending(a => a.Email).ToObservableCollection();
			}
			if (RoleAZChecked)
			{
				ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.RoleName).ToObservableCollection();
			}
			if (RoleZAChecked)
			{
				ManagerAccountList = primaryManagerAccountList.OrderByDescending(a => a.RoleName).ToObservableCollection();
			}
		}
        public void UpdateFilterList()
        {
            switch(selectedFilterAccountTypeOptions)
            {
				case "Accountant":
					ManagerAccountList = ManagerAccountList.Where(a => a.RoleName == "Accountant").ToObservableCollection();
					break;
				case "Admin":
                    ManagerAccountList = ManagerAccountList.Where(a => a.RoleName == "Admin").ToObservableCollection();
					break;
			}
        }
		partial void OnSelectedFilterAccountTypeOptionsChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && newValue != null)
            {
                UpdateSortList();
                UpdateFilterList();
                SearchFilter = "";
                ResetAccountInformation();
                ResetItemBackgrounds();
            }
        }

        partial void OnUsernameAZCheckedChanged(bool oldValue, bool newValue)
        {
			if (oldValue != newValue)
			{
                ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.Username).ToObservableCollection();
                UpdateFilterList();
				ResetAccountInformation();
				ResetItemBackgrounds();
			}
		}
		partial void OnUsernameZACheckedChanged(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
                ManagerAccountList = primaryManagerAccountList.OrderByDescending(a => a.Username).ToObservableCollection();
				UpdateFilterList(); 
                ResetAccountInformation();
				ResetItemBackgrounds();
			}
		}
		partial void OnEmailAZCheckedChanged(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
                ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.Email).ToObservableCollection();
				UpdateFilterList(); 
                ResetAccountInformation();
				ResetItemBackgrounds();
			}
		}
		partial void OnEmailZACheckedChanged(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
                ManagerAccountList = primaryManagerAccountList.OrderByDescending(a => a.Email).ToObservableCollection();
				UpdateFilterList(); 
                ResetAccountInformation();
				ResetItemBackgrounds();
			}
		}
		partial void OnRoleAZCheckedChanged(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
                ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.RoleName).ToObservableCollection();
				UpdateFilterList(); 
                ResetAccountInformation();
				ResetItemBackgrounds();
			}
		}
		partial void OnRoleZACheckedChanged(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
				ManagerAccountList = primaryManagerAccountList.OrderByDescending(a => a.RoleName).ToObservableCollection();
				UpdateFilterList(); 
                ResetAccountInformation();
				ResetItemBackgrounds();
			}
		}

		partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
			UpdateSortList();
			UpdateFilterList();
            ManagerAccountList = ManagerAccountList
                .Where(a => a.Username.Contains(newValue) || a.Email.Contains(newValue) || a.RoleName.Contains(newValue)).ToObservableCollection();
			ResetItemBackgrounds();
			ResetAccountInformation();
		}
        #endregion

        #region Helpers
        private void ReloadManagerAccountList(List<User> accountList)
        {
			primaryManagerAccountList.Clear();

            if (accountList.Count > 0)
            {
				for (int i = 0; i < accountList.Count; i++)
                {
                    primaryManagerAccountList.Add(new UserDisplay
                    {
                        Id = accountList[i].Id,
                        Username = accountList[i].Username,
                        Password = accountList[i].Password,
                        Email = accountList[i].Email,
                        RoleName = (accountList[i].Role == Role.Admin) ? "Admin" : "Accountant",
                        PrimaryRoleName = (accountList[i].Role == Role.Admin) ? "Admin" : "Accountant",
                        Avatar = (accountList[i].Role == Role.Admin) ? "admin_icon.png" : "teacher_icon.png",
                        RoleBackgroundColor = Color.FromArgb("#4F4F4F"),
                        IsVisibleDot = false,
                        UserRequester = this,
                    });
                }

                ManagerAccountList = primaryManagerAccountList.OrderBy(a => a.Username).ToObservableCollection();
                for (int i = 0; i < primaryManagerAccountList.Count; i++)
                {
                    ManagerAccountList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
                }
            }
        }

        public void ResetAccountInformation()
        {
            Id = -1;
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