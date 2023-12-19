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

		#region Demo student
		public static List<Student> students = new List<Student>()
		{
			new()
			{
				Id = 1,
				StudentSpecificId="SV21520007",
				FullName="Mai Hoàng Nhật Suy",
			},
			new()
			{
				Id = 2,
				StudentSpecificId="SV21520008",
				FullName="Mai Hoàng Nhật Duy",
			},
			new()
			{
				Id = 3,
				StudentSpecificId="SV21520009",
				FullName="Mai Hoàng Nhật Huy",
			}
		};
		#endregion

		[RelayCommand]
		public void GetCurrentStudent()
		{
			foreach (var item in students)
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
