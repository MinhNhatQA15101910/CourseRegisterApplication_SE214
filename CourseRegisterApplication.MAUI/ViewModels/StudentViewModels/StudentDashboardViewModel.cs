using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class StudentDashboardViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
		#endregion

		#region Constructor
		public StudentDashboardViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		#endregion

		[ObservableProperty]
		private string titleText = "";

		[ObservableProperty]
		private string descriptionText = "";

		[RelayCommand]
		public async Task Initial()
		{
			// Description Text
            DescriptionText = Helpers.FormatDateTime(DateTime.Now);

			// User information
            IStudentService studentService = _serviceProvider.GetService<IStudentService>();
			string currentUsername = GlobalConfig.CurrentUser.Username;

			Student currentStudent = await studentService.GetStudentBySpecificId(currentUsername);
			if (currentStudent != null)
			{
                string currentStudentName = currentStudent.FullName;
				TitleText = $"Welcome back {currentStudentName}!";
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
		public async Task NavigateToStudentInfomation()
		{
			await Shell.Current.GoToAsync("//studentinfomation/details", true);
		}

		[RelayCommand]
		public async Task NavigateToCourseRegister()
		{
			await Shell.Current.GoToAsync("//courseregistration/details", true);
		}

		[RelayCommand]
		public async Task NavigateToTuitionPayment()
		{
			await Shell.Current.GoToAsync("//tuitioninfo/details", true);
		}
	}
}
