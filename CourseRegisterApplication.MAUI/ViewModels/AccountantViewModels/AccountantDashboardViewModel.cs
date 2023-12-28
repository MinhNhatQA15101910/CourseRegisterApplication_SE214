using CourseRegisterApplication.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
	public partial class AccountantDashboardViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
		#endregion

		#region Properties
		[ObservableProperty]
		private string descriptionText = Helpers.FormatDateTime(DateTime.Now);
        #endregion

        #region Constructor
        public AccountantDashboardViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		#endregion

		#region Command
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
		public async Task NavigateToStudentManagementCommand()
		{
			await Shell.Current.GoToAsync("//studentmanagement/details", true);
		}

		[RelayCommand]
		public async Task NavigateToCourseManagementCommand()
		{
			await Shell.Current.GoToAsync("//coursemanagement/details", true);
		}

		[RelayCommand]
		public async Task NavigateToTutionCollectionCommand()
		{
			await Shell.Current.GoToAsync("//tuitioncollection/details", true);
		}
        #endregion
    }
}
