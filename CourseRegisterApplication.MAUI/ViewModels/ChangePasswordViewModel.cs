﻿using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels
{
    public partial class ChangePasswordViewModel : ObservableObject
	{
        #region Services
		private readonly IServiceProvider _serviceProvider;
		private readonly IUserService _userService;
        #endregion

        #region Properties
        [ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
		private string password1;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
		private string password2;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
		private string password3;

        [ObservableProperty]
        private string descriptionText = Helpers.FormatDateTime(DateTime.Now);
        #endregion

        #region Data Validation
        [ObservableProperty]
		private bool isLoading = false;

		[ObservableProperty]
		private string passwordMessageText1;

        [ObservableProperty]
        private Color passwordColor1;

        [ObservableProperty]
        private string passwordMessageText2;

        [ObservableProperty]
        private Color passwordColor2;

        [ObservableProperty]
        private string passwordMessageText3;

        [ObservableProperty]
        private Color passwordColor3;
        #endregion

        #region Variables
        private int globalVariable1 = 0;
        private int globalVariable2 = 0;
        private int globalVariable3 = 0;
        #endregion

        #region Constructor
        public ChangePasswordViewModel(IServiceProvider serviceProvider)
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
        public async Task NavigateBack()
        {
			await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand(CanExecute = nameof(CanChangePassword))]
		public async Task ChangePassword()
		{
			IsLoading = true;

			if (!Helpers.EncryptData(Password1).Equals(GlobalConfig.CurrentUser.Password))
			{
                IsLoading = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Your old password is incorrect. Please try again!", "Ok");
				return;
            }

			bool result = await _userService.ChangePassword(GlobalConfig.CurrentUser, Helpers.EncryptData(Password2));
			IsLoading = false;

			if (result)
			{
				GlobalConfig.CurrentUser.Password = Helpers.EncryptData(Password2);

				await Application.Current.MainPage.DisplayAlert("Success", "Change password successfully!", "Ok");
                await Shell.Current.GoToAsync("..", true);
				Clear();
            }
			else
			{
                await Application.Current.MainPage.DisplayAlert("Failed", "Change password failed!", "Ok");
            }
		}

		public bool CanChangePassword()
		{
			int index = 0;
			int index2 = 0;
			int index3 = 0;

			if (string.IsNullOrEmpty(Password1))
			{
				if (globalVariable1 == 0)
				{
					PasswordColor1 = Color.FromArgb("#FFFFFF");
				}
				else
				{
					PasswordColor1 = Color.FromArgb("#BF1D28");
				}
				PasswordMessageText1 = "Old password cannot be empty.";
				index++;
			}
			else
			{
                globalVariable1 = 1;
				PasswordColor1 = Color.FromArgb("#BF1D28");
				if (Password1.Length < 8)
				{
					PasswordMessageText1 = "Your password must be at least 8 characters.";
					index++;
				}
				else
				{
					if (Password1 == Password2)
					{
						PasswordMessageText2 = "New password must be different from old password.";
						index2++;
					}
					else
					{
						PasswordColor2 = Color.FromArgb("#007D3A");
						PasswordMessageText2 = "Valid password.";
						index2 = 0;
					}
					PasswordColor1 = Color.FromArgb("#007D3A");
					PasswordMessageText1 = "Valid password.";
					index = 0;
				}
			}
			if (string.IsNullOrEmpty(Password2))
			{
				if (globalVariable2 == 0)
				{
					PasswordColor2 = Color.FromArgb("#FFFFFF");
				}
				else
				{
					PasswordColor2 = Color.FromArgb("#BF1D28");
				}
				PasswordMessageText2 = "New password cannot be empty.";
				index2++;
			}
			else
			{
				globalVariable2 = 1;
				PasswordColor2 = Color.FromArgb("#BF1D28");
				if (Password2.Length < 8)
				{
					PasswordMessageText2 = "Your password must be at least 8 characters.";
					index2++;
				}
				else
				{
					if (Password1==Password2) 
					{
						PasswordMessageText2 = "New password must be different from old password.";
						index2++;
					}
					else
					{
						PasswordColor2 = Color.FromArgb("#007D3A");
						PasswordMessageText2 = "Valid password.";
						index2 = 0;
					}
				}
			}
			if (string.IsNullOrEmpty(Password3))
			{
				if (globalVariable3 == 0)
				{
					PasswordColor3 = Color.FromArgb("#FFFFFF");
				}
				else
				{
					PasswordColor3 = Color.FromArgb("#BF1D28");
				}
				PasswordMessageText3 = "Confirm password cannot be empty.";
				index3++;
			}
			else
			{
				globalVariable3 = 1;
				PasswordColor3 = Color.FromArgb("#BF1D28");
				if (Password3.Length < 8)
				{
					PasswordMessageText3 = "Your password must be at least 8 characters.";
					index3++;
				}
				else
				{
					if (Password2 != Password3)
					{
						PasswordMessageText3 = "Confirm password doesn't match with new password.";
						index3++;
					}
					else
					{
						PasswordColor3 = Color.FromArgb("#007D3A");
						PasswordMessageText3 = "Valid password.";
						index3 = 0;
					}
				}
			}
			if (index > 0 || index2 > 0 || index3 > 0)
			{
                return false;
            }

			return true;
		}

		private void Clear()
		{
			Password1 = "";
			Password2 = "";
			Password3 = "";
		}
	}
}
