using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
	public partial class StudentDashboardViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
		private readonly IUserService _userService;
		private readonly IStudentService _studentService;
		#endregion

		#region Constructor
		public StudentDashboardViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_userService = serviceProvider.GetService<IUserService>();
			_studentService = serviceProvider.GetService<IStudentService>();
		}
		#endregion

		[ObservableProperty]
		private string titleText = "";

		[ObservableProperty]
		private string descriptionText = "";

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
		public async Task GetCurrentStudent()
		{
			DescriptionText = Helpers.FormatDateTime(DateTime.Now);
			string CurrentUsername = GlobalConfig.CurrentUser.Username;
			//var studentList = await _studentService.GetStudents();
			foreach(var item in students)
			{
				if (item.StudentSpecificId == CurrentUsername)
				{
					TitleText = "Welcome back " + item.FullName + "!";
					
					break;
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
			await Shell.Current.GoToAsync("//tuitionpayment/details", true);
		}

	}
}
