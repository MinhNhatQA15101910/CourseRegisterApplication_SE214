using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;
using System;

namespace CourseRegisterApplication.MAUI.ViewModels
{
	public partial class ChangePasswordViewModel : ObservableObject
	{
		private readonly ChangePasswordPage _changePasswordPage;
		private readonly IUserService _userService = new UserService();

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
		private bool isLoading = false;

		public ChangePasswordViewModel(ChangePasswordPage changePasswordPage)
		{
			_changePasswordPage = changePasswordPage;
		}

		[RelayCommand(CanExecute = nameof(CanChangePassword))]
		public async Task ChangePassword()
		{
			IsLoading = true;
			User user = await _userService.ChangePassword(Helpers.EncryptData(Password1));
		}

		private string _passwordMessageText1;
		public string PasswordMessageText1
		{
			get { return _passwordMessageText1; }
			set
			{
				if (_passwordMessageText1 != value)
				{
					_passwordMessageText1 = value;
					OnPropertyChanged(nameof(PasswordMessageText1));
				}
			}
		}

		private Color _passwordColor1;
		public Color PasswordColor1
		{
			get { return _passwordColor1; }
			set
			{
				if (_passwordColor1 != value)
				{
					_passwordColor1 = value;
					OnPropertyChanged(nameof(PasswordColor1));
				}
			}
		}

		private string _passwordMessageText2;
		public string PasswordMessageText2
		{
			get { return _passwordMessageText2; }
			set
			{
				if (_passwordMessageText2 != value)
				{
					_passwordMessageText2 = value;
					OnPropertyChanged(nameof(PasswordMessageText2));
				}
			}
		}

		private Color _passwordColor2;
		public Color PasswordColor2
		{
			get { return _passwordColor2; }
			set
			{
				if (_passwordColor2 != value)
				{
					_passwordColor2 = value;
					OnPropertyChanged(nameof(PasswordColor2));
				}
			}
		}

		private string _passwordMessageText3;
		public string PasswordMessageText3
		{
			get { return _passwordMessageText3; }
			set
			{
				if (_passwordMessageText3 != value)
				{
					_passwordMessageText3 = value;
					OnPropertyChanged(nameof(PasswordMessageText3));
				}
			}
		}

		private Color _passwordColor3;
		public Color PasswordColor3
		{
			get { return _passwordColor3; }
			set
			{
				if (_passwordColor3 != value)
				{
					_passwordColor3 = value;
					OnPropertyChanged(nameof(PasswordColor3));
				}
			}
		}


		public static int globalVariable = 0;
		public static int globalVariable2 = 0;
		public static int globalVariable3 = 0;

		public bool CanChangePassword()
		{
			int index = 0;
			int index2 = 0;
			int index3 = 0;

			if (string.IsNullOrEmpty(Password1))
			{
				if (globalVariable == 0)
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
				globalVariable = 1;
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
			if (index > 0 || index2 > 0 || index3>0) return false;
			else return true;
		}

		private void Clear()
		{
			IsLoading = false;
			Password1 = "";
			Password2 = "";
			Password3 = "";
		}
	}
}
