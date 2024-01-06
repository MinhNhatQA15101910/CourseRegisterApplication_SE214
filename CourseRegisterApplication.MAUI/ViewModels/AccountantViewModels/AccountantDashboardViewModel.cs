using CourseRegisterApplication.MAUI.Views;

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
		public async Task NavigateToStudentManagement()
		{
			await Shell.Current.GoToAsync("//studentmanagement/details", true);
		}

		[RelayCommand]
		public async Task NavigateToCourseManagement()
		{
			await Shell.Current.GoToAsync("//courseconfirmmanagement/details", true);
		}

		[RelayCommand]
		public async Task NavigateToTuitionCollection()
		{
			await Shell.Current.GoToAsync("//tuitionconfirm/details", true);
		}
		#endregion
	}
}
