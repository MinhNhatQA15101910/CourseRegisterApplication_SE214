using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;


namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class StudentAppShellViewModel : ObservableObject
    {
		#region Services
		private readonly IServiceProvider _serviceProvider;
		#endregion

		#region Constructor
		public StudentAppShellViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		#endregion

		#region Properties
		[ObservableProperty] private string avatar;

		[ObservableProperty] private string studentName;
        #endregion

        #region Commands
        [RelayCommand]
		public async Task NavigateToChangePasswordPage()
		{
			if (Shell.Current.CurrentPage is not ChangePasswordPage)
			{
				await Shell.Current.GoToAsync(nameof(ChangePasswordPage), true);
			}
		}
		#endregion

		[RelayCommand]
		public async Task GetCurrentStudent()
		{
			IStudentService studentService = _serviceProvider.GetService<IStudentService>();

			string studentSpecificId = GlobalConfig.CurrentUser.Username;
			Student student = await studentService.GetStudentBySpecificId(studentSpecificId);
			if (student != null)
			{
				Avatar = student.ImageUrl;
				StudentName = student.FullName;
			}
		}
	}
}
