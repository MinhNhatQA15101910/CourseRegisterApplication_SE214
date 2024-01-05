using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AddManagerAccountViewModel : ObservableObject
    {
		#region Services
		private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        [ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(AddAccountCommand))]
		private string username;

		[ObservableProperty]
		private string usernameMessageText;

		[ObservableProperty]
		private Color usernameColor;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(AddAccountCommand))]
		private string password;

		[ObservableProperty]
		private string passwordMessageText;

		[ObservableProperty]
		private Color passwordColor;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddAccountCommand))]
        private string passwordConfirm;

        [ObservableProperty]
        private string passwordConfirmMessageText;

        [ObservableProperty]
        private Color passwordConfirmColor;

        [ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(AddAccountCommand))]
		private string email;

		[ObservableProperty]
		private string emailMessageText;

        [ObservableProperty]
        private Color emailColor;

        [ObservableProperty]
		private bool isLoading = false;

        [ObservableProperty]
        private string descriptionText = Helpers.FormatDateTime(DateTime.Now);

		[ObservableProperty]
		private ObservableCollection<string> roles = new() { "Admin", "Accountant" };

		[ObservableProperty]
		private string selectedRole = "Admin";
        #endregion

        #region Variables
        private int globalVariable1;
		private int globalVariable2;
		private int globalVariable3;
		private int globalVariable4;
        #endregion

        #region Constructor
        public AddManagerAccountViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_userService = serviceProvider.GetService<IUserService>();
		}
        #endregion

        #region Commands
        [RelayCommand(CanExecute = nameof(CanAddAccount))]
		public async Task AddAccount()
		{
			bool accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this user?", "Yes", "No");
            if (accept)
            {
                IsLoading = true;

                // If there is already a user with the same username with the input, display error
                List<User> users = await _userService.GetAllUsers();
                User user = users.Find(u => u.Username == Username);
                if (user != null)
                {
					IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Error", "Username has already existed. Please try another!", "OK");
                    return;
                }

                user = new User
                {
                    Username = Username,
                    Password = Helpers.EncryptData(Password),
                    Email = Email,
                    Role = (SelectedRole == "Admin") ? Role.Admin : Role.Accountant
                };

                User resultUser = await _userService.AddUser(user);
                if (resultUser != null)
                {
					IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Success", "Add user successfully!", "OK");
                    Clear();
                }
                else
                {
					IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Failed", "Error occurred!", "OK");
                }
            }
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

        [RelayCommand]
        public async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }
        #endregion

        #region Can Execute Commands
        public bool CanAddAccount()
		{
			int index1 = 0;
			int index2 = 0;
			int index3 = 0;
            int index4 = 0;

			// Validate Username
			if (string.IsNullOrEmpty(Username))
			{
				if (globalVariable1 == 0)
				{
					UsernameColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					UsernameColor = Color.FromArgb("#BF1D28");
				}
				UsernameMessageText = "Username cannot be empty.";
				index1++;
			}
			else
			{
                globalVariable1 = 1;
				UsernameColor = Color.FromArgb("#BF1D28");
				if (Username.Length < 3)
				{
					UsernameMessageText = "Your username must be at least 3 characters.";
					index1++;
				}
				else
				{
					UsernameColor = Color.FromArgb("#007D3A");
					UsernameMessageText = "Valid Username.";
					index1 = 0;
				}
			}

			// Validate Password
			if (string.IsNullOrEmpty(Password))
			{
				if (globalVariable2 == 0)
				{
					PasswordColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					PasswordColor = Color.FromArgb("#BF1D28");
				}
				PasswordMessageText = "Password cannot be empty.";
				index2++;
			}
			else
			{
				globalVariable2 = 1;
				PasswordColor = Color.FromArgb("#BF1D28");
				if (Password.Length < 8)
				{
					PasswordMessageText = "Your password must be at least 8 characters.";
					index2++;
				}
				else
				{
					PasswordColor = Color.FromArgb("#007D3A");
					PasswordMessageText = "Valid Password.";
					index2 = 0;
				}
			}

			// Validate confirm password
            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                if (globalVariable3 == 0)
                {
                    PasswordConfirmColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    PasswordConfirmColor = Color.FromArgb("#BF1D28");
                }
                PasswordConfirmMessageText = "Password confirm cannot be empty.";
                index3++;
            }
            else
            {
                globalVariable3 = 1;
                PasswordConfirmColor = Color.FromArgb("#BF1D28");
                if (!PasswordConfirm.Equals(Password))
                {
                    PasswordConfirmMessageText = "Password confirm is not match with your password!";
                    index3++;
                }
                else
                {
                    PasswordConfirmColor = Color.FromArgb("#007D3A");
                    PasswordConfirmMessageText = "Valid Password Confirm.";
                    index3 = 0;
                }
            }

            // Validate Email
            if (string.IsNullOrEmpty(Email))
			{
				if (globalVariable4 == 0)
				{
					EmailColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					EmailColor = Color.FromArgb("#BF1D28");
				}
				EmailMessageText = "Email cannot be empty.";
				index4++;
			}
			else
			{
				globalVariable4 = 1;
				EmailColor = Color.FromArgb("#BF1D28");
				if (!ValidateEmail(Email))
				{
                    EmailMessageText = "Email is not in a valid format!";
                    index4++;
                }
				else
				{
					EmailColor = Color.FromArgb("#007D3A");
					EmailMessageText = "Valid Email.";
					index4 = 0;
				}
			}

			if (index1 > 0 || index2 > 0 || index3 > 0 || index4 > 0)
			{
				return false;
			}

			return true;
		}
        #endregion

        #region Helpers
        private void Clear()
		{
			Username = "";
			Password = "";
			PasswordConfirm = "";
			Email = "";
			SelectedRole = "Admin";
		}

		private bool ValidateEmail(string email)
		{
			try
			{
                if (email.EndsWith('.'))
                {
                    return false;
                }

                var address = new MailAddress(email);
                return address.Address == email;
            }
			catch
			{
				return false;
			}
		}
        #endregion
    }
}
