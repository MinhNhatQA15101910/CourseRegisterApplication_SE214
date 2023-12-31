using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class SelectedPriorityTypeDisplay : ObservableObject
    {
        [ObservableProperty] private string priorityName;
    }

    public partial class AddUpdateStudentViewModel : ObservableObject
    {
        #region Service
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string imageUrl = "https://static.wixstatic.com/media/8027bc_6d79e9c44bae49de97c018a781738884~mv2.jpg/v1/fill/w_987,h_1096,al_c,q_90/file.jpg";

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string studentId;

        [ObservableProperty] private string studentIdMessageText;

        [ObservableProperty] private Color studentIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string studentName;

        [ObservableProperty] private string studentNameMessageText;

        [ObservableProperty] private Color studentNameColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string email;

        [ObservableProperty] private string emailMessageText;

        [ObservableProperty] private Color emailColor;

        [ObservableProperty] private DateTime dateOfBirth;

        [ObservableProperty] private ObservableCollection<string> genderList = new() { "Male", "Female" };

        [ObservableProperty] private ObservableCollection<Province> provinceList = new() { new() { Id = 0, ProvinceName = "- Select province -" } };

        [ObservableProperty] private ObservableCollection<District> districtList = new() { new() { Id = 0, DistrictName = "- Select district -" } };

        [ObservableProperty] private ObservableCollection<Department> departmentList = new() { new() { Id = 0, DepartmentName = "- Select department -" } };

        [ObservableProperty] private ObservableCollection<Branch> branchList = new() { new() { Id = 0, BranchName = "- Select branch -" } };

        private List<PriorityType> primaryPriorityTypeList = new();

        [ObservableProperty] private ObservableCollection<PriorityType> priorityTypeList = new() { new() { Id = 0, PriorityName = "- Select priority type -" } };

        [ObservableProperty] private ObservableCollection<SelectedPriorityTypeDisplay> selectedPriorityTypeList = new();

        [ObservableProperty] private string selectedGender = "Male";

        [ObservableProperty] private Province selectedProvince;

        [ObservableProperty] private District selectedDistrict;

        [ObservableProperty] private Branch selectedBranch;

        [ObservableProperty] private Department selectedDepartment;

        [ObservableProperty] private PriorityType selectedPriorityType;

        [ObservableProperty] private bool isLoading;
        #endregion

        #region Constructor
        public AddUpdateStudentViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            // Department
            IDepartmentService departmentService = _serviceProvider.GetService<IDepartmentService>();
            DepartmentList = (await departmentService.GetAllDepartments()).ToObservableCollection();
            DepartmentList.Insert(0, new() { Id = 0, DepartmentName = "- Select department -" });

            SelectedDepartment = DepartmentList[0];

            // Branch
            BranchList.Add(new() { Id = 0, BranchName = "- Select branch -" });

            SelectedBranch = BranchList[0];

            // Province
            IProvinceService provinceService = _serviceProvider.GetService<IProvinceService>();
            ProvinceList = (await provinceService.GetAllProvinces()).ToObservableCollection();
            ProvinceList.Insert(0, new() { Id = 0, ProvinceName = "- Select province -" });

            SelectedProvince = ProvinceList[0];

            // District
            DistrictList.Add(new() { Id = 0, DistrictName = "- Select district -" });

            SelectedDistrict = DistrictList[0];

            // Priority Types
            IPriorityTypeService priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
            primaryPriorityTypeList = (await priorityTypeService.GetAllPriorityTypesAsync()).ToList();

            PriorityType nonePriorityType = primaryPriorityTypeList.First(pt => pt.Id == 1);
            SelectedPriorityTypeList.Add(new() { PriorityName = nonePriorityType.PriorityName });

            PriorityTypeList = primaryPriorityTypeList
                .Where(pt => pt.Id != 1 && pt.Id != 2)
                .ToObservableCollection();
            PriorityTypeList.Insert(0, new() { Id = 0, PriorityName = "- Select priority type -" });

            SelectedPriorityType = PriorityTypeList[0];
        }

        [RelayCommand(CanExecute = nameof(CanAddStudentExecuted))]
        public async Task AddStudent()
        {
            //bool accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this student?", "Yes", "No");
            //if (accept)
            //{
            //    IsLoading = true;

            //    // If there is already a user with the same username with the input, display error
            //    List<Student> students = await _studentService.GetAllStudents();
            //    Student student = students.Find(s => s.StudentSpecificId == StudentId);
            //    if (student != null)
            //    {
            //        IsLoading = false;
            //        await Application.Current.MainPage.DisplayAlert("Error", "Student has already existed!", "OK");
            //        return;
            //    }

            //    student = new Student
            //    {
            //        StudentSpecificId = StudentId,
            //        FullName = StudentName,
            //        Gender = (SelectedGender == "Male") ? Gender.Male : Gender.Female,
            //        DateOfBirth = DateOfBirth,
            //        DistrictId = SelectedDistrict.Id,
            //        District = SelectedDistrict,
            //        BranchId = SelectedBranch.Id,
            //        Branch = SelectedBranch
            //    };

            //    Student resultStudent = await _studentService.AddStudent(student);
            //    if (resultStudent != null)
            //    {
            //        IsLoading = false;
            //        await Application.Current.MainPage.DisplayAlert("Success", "Add student successfully!", "OK");
            //        Clear();
            //    }
            //    else
            //    {
            //        IsLoading = false;
            //        await Application.Current.MainPage.DisplayAlert("Failed", "Error occurred!", "OK");
            //    }
            //}
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
            //var branches = await _branchService.GetBranchesByDepartmentId(SelectedDepartment.Id);
            //BranchList.Clear();
            //foreach (var branch in branches)
            //{
            //    BranchList.Add(branch);
            //}
            //BranchEnable = true;
        }

        [RelayCommand]
        public async Task GetDistrict()
        {
            //var districts = await _districtService.GetDistrictsByProvinceId(SelectedProvince.Id);
            //DistrictList.Clear();
            //foreach (var district in districts)
            //{
            //    DistrictList.Add(district);
            //}
            //DistrictEnable = true;
        }

        partial void OnSelectedDepartmentChanged(Department value)
        {
            // GetBranchCommand.Execute(null);
        }

        partial void OnSelectedProvinceChanged(Province value)
        {
            // getDistrictCommand.Execute(null);
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
        public void Clear()
        {
            StudentName = "";
            Email = "";
            SelectedGender = "Male";
            SelectedProvince = ProvinceList[0];
            SelectedDistrict = DistrictList[0];
            SelectedBranch = BranchList[0];
            SelectedDepartment = DepartmentList[0];
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
