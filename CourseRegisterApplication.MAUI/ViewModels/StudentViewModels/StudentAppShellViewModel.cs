using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.IServices;


namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class StudentAppShellViewModel : ObservableObject
    {
		#region Services
		private readonly IServiceProvider _serviceProvider;
		private readonly IUserService _userService;
		private readonly IStudentService _studentService;
		#endregion

		#region Constructor
		public StudentAppShellViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_userService = serviceProvider.GetService<IUserService>();
			_studentService = serviceProvider.GetService<IStudentService>();
		}
		#endregion

		[ObservableProperty]
		private string avatar = "demo_icon.png";

		[ObservableProperty]
		private string studentName = "";

		[RelayCommand]
		public async Task NavigateToChangePasswordPage()
		{
			if (Shell.Current.CurrentPage is not ChangePasswordPage)
			{
				await Shell.Current.GoToAsync(nameof(ChangePasswordPage), true);
			}
		}

		[RelayCommand]
		public async Task GetCurrentStudent()
		{
			List<Student> studentList = await _studentService.GetStudents();

            foreach (var item in studentList)
			{
				if (item.StudentSpecificId == GlobalConfig.CurrentUser.Username)
				{
					StudentName = item.FullName;
					break;
				}
			}
		}
	}
}
