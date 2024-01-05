using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.StudentViews;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class StudentInfomationViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
		private readonly FirebaseStorage _firebaseStorage = new FirebaseStorage("courseregistrationfirebase.appspot.com");
		#endregion

        #region Constructor
        public StudentInfomationViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		#endregion

		#region Properties
		[ObservableProperty] private string imageUrl;

		[ObservableProperty] private string studentName;

		[ObservableProperty] private string studentID;

		[ObservableProperty] private string studentGender;

		[ObservableProperty] private string studentAge;

		[ObservableProperty] private string studentBranchName;

		[ObservableProperty] private string studentDepartmentName;

		[ObservableProperty] private string studentDOB;

		[ObservableProperty] private string studentEmail;

		[ObservableProperty] private string studentDistrictProvince;

		[ObservableProperty] private ObservableCollection<PriorityType> studentPriorityTypeList = new();
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
		public async Task GetCurrentStudentInfomation()
		{
			IStudentService studentService = _serviceProvider.GetService<IStudentService>();
			User currentUser = GlobalConfig.CurrentUser;
			string currentUsername = currentUser.Username;

			Student currentStudent = await studentService.GetFullInformationOfStudentBySpecificId(currentUsername);
			if (currentStudent != null)
			{
				ImageUrl = currentStudent.ImageUrl;
				StudentName = currentStudent.FullName;
				StudentID = currentStudent.StudentSpecificId;
				StudentGender = (currentStudent.Gender == Gender.Male) ? "Male" : "Female";
				StudentAge = Helpers.CalculateAge(currentStudent.DateOfBirth).ToString();
				StudentDOB = currentStudent.DateOfBirth.ToString("dd/MM/yyyy");
				StudentEmail = currentStudent.Email;
				StudentBranchName = currentStudent.Branch.BranchName;
				StudentDepartmentName = currentStudent.Branch.Department.DepartmentName;
				StudentDistrictProvince = $"{currentStudent.District.DistrictName}, {currentStudent.District.Province.ProvinceName}";

				// Get Priority Type List
				IPriorityTypeService priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
                StudentPriorityTypeList = (await priorityTypeService.GetPriorityTypesFromStudentIdAsync(currentStudent.Id)).ToObservableCollection();
			}
		}

		[RelayCommand]
		public async Task ChooseAndUploadImage()
		{
			var result = await FilePicker.Default.PickAsync(new PickOptions
			{
				PickerTitle = "Please select an image file.",
				FileTypes = FilePickerFileType.Images
			});

            if (result != null && 
				(result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
				result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("svg", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("gif", StringComparison.OrdinalIgnoreCase)))
            {
                // Upload Image
                using var stream = await result.OpenReadAsync();
                var newImageUrl = await _firebaseStorage
                    .Child("Images")
                    .Child("Avatar")
                    .Child($"{GlobalConfig.CurrentUser.Username}-Avatar")
                    .PutAsync(stream);

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    // Update UI
                    ImageUrl = newImageUrl;

                    // Update database
                    IStudentService studentService = _serviceProvider.GetService<IStudentService>();
                    Student currentStudent = await studentService.GetStudentBySpecificId(GlobalConfig.CurrentUser.Username);
                    await studentService.UpdateImageUrl(currentStudent, newImageUrl);

                    // Reload information in StudentAppShell
                    StudentAppShellViewModel studentAppShellViewModel = _serviceProvider.GetService<StudentAppShellViewModel>();
					await studentAppShellViewModel.GetCurrentStudentCommand.ExecuteAsync(null);
                }
            }
        }
        #endregion
    }
}
