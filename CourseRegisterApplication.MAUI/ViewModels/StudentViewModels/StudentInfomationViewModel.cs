using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.ViewModels.AdminViewModels;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{


	public partial class StudentInfomationViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
		private readonly IStudentService _studentService;
		#endregion

		#region Constructor
		public StudentInfomationViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_studentService = serviceProvider.GetService<IStudentService>();
		}
		#endregion

		#region Demo student
		public static List<StudentPriorityType> studentPriorityTypes = new List<StudentPriorityType>()
		{
			new()
			{
				StudentId=1,
				PriorityTypeId=1
			},
			new()
			{
				StudentId=1,
				PriorityTypeId=2
			},
			new()
			{
				StudentId=1,
				PriorityTypeId=3
			},
			new()
			{
				StudentId=1,
				PriorityTypeId=4
			},
		};

		public static List<PriorityType> priorityTypes = new List<PriorityType>()
		{
			new()
			{
				Id=1,
				PriorityName="Vùng sâu vùng xa"
			},
			new()
			{
				Id=2,
				PriorityName="Con thương binh"
			},
			new()
			{
				Id=3,
				PriorityName="Con liệt sỹ"
			},
			new()
			{
				Id=4,
				PriorityName="Con ông cháu cha"
			}
		};

		public static List<Branch> branchs = new List<Branch>()
		{
			new()
			{
				Id=1,
				DepartmentId=1,
				BranchSpecificId="KHMT",
				BranchName="Khoa học máy tính"
			}
		};
		public static List<Department> departments = new List<Department>()
		{
			new()
			{
				Id=1,
				DepartmentSpecificId="KHMT",
				DepartmentName="Khoa Học Máy Tính"
			}
		};
		public static List<District> districts = new List<District>()
		{
			new()
			{
				Id=1,
				DistrictName="Thống Nhất",
				ProvinceId=1
			}
		};
		public static List<Province> provinces = new List<Province>()
		{
			new()
			{
				Id=1,
				ProvinceName="Đồng Nai"
			}
		};
		public static List<Student> students = new List<Student>()
		{
			new()
			{
				Id = 1,
				StudentSpecificId="SV21520007",
				FullName="Mai Hoàng Nhật Suy",
				Gender=Gender.Male,
				DateOfBirth=new DateTime(2003, 5, 15),
				BranchId=1,
				DistrictId=1,
			}
		};

		

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
		public void GetCurrentStudentInfomation()
		{
			foreach (var item in students)
			{
				if (item.StudentSpecificId == GlobalConfig.CurrentUser.Username)
				{
					StudentName = item.FullName;
					StudentID = item.StudentSpecificId;
					StudentGender = item.Gender.ToString();
					StudentAge = Helpers.CalculateAge(item.DateOfBirth).ToString();
					foreach(var item2 in studentPriorityTypes) 
					{
						if(item2.StudentId==item.Id)
						{
							foreach(var item3 in priorityTypes)
							{
								if(item3.Id == item2.PriorityTypeId)
								{
									StudentPriorityObjectList.Add(item3.PriorityName);
								}
							}
							
						}
					}
					
					break;
				}

			}
		}
	}
}
