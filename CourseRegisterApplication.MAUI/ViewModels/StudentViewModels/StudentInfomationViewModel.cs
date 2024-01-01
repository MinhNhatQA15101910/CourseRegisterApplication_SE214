﻿using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{


	public partial class StudentInfomationViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
		private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
		private readonly IBranchService _branchService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IStudentPriorityTypeService _studentPriorityTypeService;
        private readonly IPriorityType _priorityType;
        #endregion

        #region Constructor
        public StudentInfomationViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_studentService = serviceProvider.GetService<IStudentService>();
            _departmentService = serviceProvider.GetService<IDepartmentService>();
            _branchService = serviceProvider.GetService<IBranchService>();
            _provinceService = serviceProvider.GetService<IProvinceService>();
            _districtService = serviceProvider.GetService<IDistrictService>();
            _studentPriorityTypeService = serviceProvider.GetService<IStudentPriorityTypeService>();
			_priorityType = serviceProvider.GetService<IPriorityType>();
        }
		#endregion

		#region Properties
		[ObservableProperty]
		private string descriptionText = "View student infomation";

		[ObservableProperty]
		private string titleText = "Student infomation page";

		[ObservableProperty]
		private string studentName = "";

		[ObservableProperty]
		private string studentID = "";

		[ObservableProperty]
		private string studentGender = "";

		[ObservableProperty]
		private string studentAge = "";

		[ObservableProperty]
		private string studentBranch = "";

		[ObservableProperty]
		private string studentDepartment = "";

		[ObservableProperty]
		private string studentDOB = "";

		[ObservableProperty]
		private string studentEmail = "";

		[ObservableProperty]
		private string studentDistrictProvince = "";

		[ObservableProperty]
		private ObservableCollection<string> studentPriorityObjectList = new();

		[ObservableProperty]
		private string studentPriorityObjectItem = "";

		#endregion

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
            List<Student> studentList = await _studentService.GetStudents();
            foreach (var item in studentList)
			{
				if (item.StudentSpecificId == GlobalConfig.CurrentUser.Username)
				{
					StudentName = item.FullName;
					StudentID = item.StudentSpecificId;
					StudentGender = item.Gender.ToString();
					StudentAge = Helpers.CalculateAge(item.DateOfBirth).ToString();
					StudentDOB = item.DateOfBirth.Date.ToString("dd/MM/yyyy");
					StudentEmail = item.StudentSpecificId + "@gm.uit.edu.vn";

                    Branch branch = await _branchService.GetBranchById(item.BranchId);
                    StudentBranch = branch.BranchName;

                    Department department = await _departmentService.GetDepartmentById(branch.DepartmentId);
					StudentDepartment = department.DepartmentName;

					District district = await _districtService.GetDistrictById(item.DistrictId);
					Province province = await _provinceService.GetProvinceById(district.ProvinceId);
					StudentDistrictProvince = district.DistrictName + ", " + province.ProvinceName;
                    List <StudentPriorityType> priorityList = await _studentPriorityTypeService.GetStudentPriorityTypesByStudentId(item.Id);
					if (priorityList.Count > 0)
					{
						foreach(var item2 in  priorityList)
						{
                            StudentPriorityObjectList.Add(item2.PriorityType.PriorityName.ToString());
                        }
					}
					
					break;
				}

			}
		}
	}
}
