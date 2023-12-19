using CourseRegisterApplication.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
	public partial class AccountantAppShellViewModel : ObservableObject
	{
		[ObservableProperty]
		private string username = GlobalConfig.CurrentUser.Username;

		[RelayCommand]
		public async Task NavigateToChangePasswordPage()
		{
			if (Shell.Current.CurrentPage is not ChangePasswordPage)
			{
				await Shell.Current.GoToAsync(nameof(ChangePasswordPage), true);
			}
		}
	}
}
