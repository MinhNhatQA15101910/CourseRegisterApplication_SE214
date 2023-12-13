using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class AddStudentViewModel : ObservableObject
    {
        #region Service
        private readonly IServiceProvider _serviceProvider;
        private readonly IStudentService _studentService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;

        #endregion

        #region Properties

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string studentId;

        [ObservableProperty]
        private string studentIdMessageText;

        [ObservableProperty]
        private Color studentIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string studentName;

        [ObservableProperty]
        private string studentNameMessageText;

        [ObservableProperty]
        private Color studentNameColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string email;

        [ObservableProperty]
        private string emailMessageText;

        [ObservableProperty]
        private Color emailColor;

        [ObservableProperty]
        private DateTime dateOfBirth;

        [ObservableProperty]
        private ObservableCollection<string> genderList = new() { "Male", "Female" };

        [ObservableProperty]
        private ObservableCollection<Province> provinceList = new();

        [ObservableProperty]
        private ObservableCollection<District> districtList = new();

        [ObservableProperty]
        private ObservableCollection<Department> departmentList = new();

        [ObservableProperty]
        private ObservableCollection<Branch> branchList = new();

        [ObservableProperty]
        private string selectedGender;

        [ObservableProperty]
        private Province selectedProvince;

        [ObservableProperty]
        private District selectedDistrict;

        [ObservableProperty]
        private Branch selectedBranch;

        [ObservableProperty]
        private Department selectedDepartment;

        [ObservableProperty]
        private bool districtEnable = false;

        [ObservableProperty]
        private bool branchEnable = false;

        [ObservableProperty]
        private bool isLoading = false;

        #endregion

        #region Constructor
        public AddStudentViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _studentService = _serviceProvider.GetRequiredService<IStudentService>();
            _departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            _branchService = _serviceProvider.GetRequiredService<IBranchService>();
            _provinceService = _serviceProvider.GetRequiredService<IProvinceService>();
            _districtService = _serviceProvider.GetRequiredService<IDistrictService>();
        }
        #endregion

        #region Variables 
        private int globalVariable1 = 0;
        private int globalVariable2 = 0;
        private int globalVariable3 = 0;
        #endregion

        #region Commands

        [RelayCommand]
        public async Task GetInformation()
        {
            //Department
            var departments = await _departmentService.GetAllDepartments();
            foreach (var department in departments)
            {
                DepartmentList.Add(department);
            }

            //Province
            /*var provinces = await _provinceService.GetAllProvince();
            foreach (var province in provinces)
            {
                Province.Add(province);
            }*/
        }

        [RelayCommand(CanExecute = nameof(CanAddStudentExecuted))]
        public async Task AddStudent()
        {
            bool accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this student?", "Yes", "No");
            if (accept)
            {
                IsLoading = true;

                // If there is already a user with the same username with the input, display error
                List<Student> students = await _studentService.GetStudents();
                Student student = students.Find(s => s.StudentSpecificId == StudentId);
                if (student != null)
                {
                    IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Error", "Student has already existed!", "OK");
                    return;
                }

                student = new Student
                {
                    StudentSpecificId = StudentId,
                    FullName = StudentName,
                    Gender = (SelectedGender == "Male") ? Gender.Male : Gender.Female,
                    DateOfBirth = DateOfBirth,
                    DistrictId = SelectedDistrict.Id,
                    District = SelectedDistrict,
                    BranchId = SelectedBranch.Id,
                    Branch = SelectedBranch
                };

                Student resultStudent = await _studentService.AddStudent(student);
                if (resultStudent != null)
                {
                    IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Success", "Add student successfully!", "OK");
                    Clear();
                }
                else
                {
                    IsLoading = false;
                    await Application.Current.MainPage.DisplayAlert("Failed", "Error occurred!", "OK");
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
        public async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand]
        public async Task GetBranch()
        {
            var branches = await _branchService.GetBranchesByDepartmentId(SelectedDepartment.Id);
            BranchList.Clear();
            foreach (var branch in branches)
            {
                BranchList.Add(branch);
            }
            BranchEnable = true;
        }

        [RelayCommand]
        public async Task GetDistrict()
        {
            var districts = await _districtService.GetDistrictsByProvinceID(SelectedProvince.Id);
            DistrictList.Clear();
            foreach (var district in districts)
            {
                DistrictList.Add(district);
            }
            DistrictEnable = true;
        }

        partial void OnSelectedDepartmentChanged(Department value)
        {
            GetBranchCommand.Execute(null);
        }

        partial void OnSelectedProvinceChanged(Province value)
        {
            getDistrictCommand.Execute(null);
        }

        public bool CanAddStudentExecuted()
        {
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;

            //validate StudentId
            if (string.IsNullOrEmpty(StudentId))
            {
                if (globalVariable1 == 0)
                {
                    StudentIdColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    StudentIdColor = Color.FromArgb("#BF1D28");
                }
                StudentIdMessageText = "Student ID cannot be empty.";
                index1++;
            }
            else
            {
                globalVariable1 = 1;
                StudentIdColor = Color.FromArgb("#007D3A");
                StudentIdMessageText = "Valid student ID.";
                index1 = 0;
            }

            //validate StudentName
            if (string.IsNullOrEmpty(StudentName))
            {
                if (globalVariable2 == 0)
                {
                    StudentNameColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    StudentNameColor = Color.FromArgb("#BF1D28");
                }
                StudentNameMessageText = "Student name cannot be empty.";
                index2++;
            }
            else
            {
                globalVariable2 = 1;
                StudentNameColor = Color.FromArgb("#007D3A");
                StudentNameMessageText = "Valid student name.";
                index2 = 0;
            }

            //validate Email
            if (string.IsNullOrEmpty(Email))
            {
                if (globalVariable3 == 0)
                {
                    EmailColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    EmailColor = Color.FromArgb("#BF1D28");
                }
                EmailMessageText = "Email cannot be empty.";
                index3++;
            }
            else
            {
                globalVariable3 = 1;
                EmailColor = Color.FromArgb("#007D3A");
                EmailMessageText = "Valid email.";
                index3 = 0;
            }


            if (index1 > 0 || index2 > 0 || index3 > 0)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Helper

        private void Clear()
        {
            StudentName = "";
            Email = "";
            SelectedGender = "";
            SelectedProvince = null;
            SelectedDistrict = null;
            SelectedBranch = null;
            SelectedDepartment = null;
            DistrictEnable = false;
            BranchEnable = false;
        }

        private bool ValidateEmail(string email)
        {
            try
            {
                if (email.EndsWith('.'))
                {
                    return false;
                }

                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
