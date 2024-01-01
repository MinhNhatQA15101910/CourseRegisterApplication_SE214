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
        private string studentSpecificId;

        [ObservableProperty] private string studentSpecificIdMessageText;

        [ObservableProperty] private Color studentSpecificIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string studentName;

        [ObservableProperty] private string studentNameMessageText;

        [ObservableProperty] private Color studentNameColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddStudentCommand))]
        private string email;

        [ObservableProperty] private string emailMessageText;

        [ObservableProperty] private Color emailColor;

        [ObservableProperty] private DateTime dateOfBirth = DateTime.Now;

        [ObservableProperty] private ObservableCollection<string> genderList = new() { "Male", "Female" };

        [ObservableProperty] private ObservableCollection<Province> provinceList = new();

        [ObservableProperty] private ObservableCollection<District> districtList = new();

        [ObservableProperty] private ObservableCollection<Department> departmentList = new();

        [ObservableProperty] private ObservableCollection<Branch> branchList = new();

        private List<PriorityType> primaryPriorityTypeList = new();

        [ObservableProperty] private ObservableCollection<PriorityType> priorityTypeList = new();

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
        public async Task GetInformation(string studentSpecificId)
        {
            // Add student
            if (string.IsNullOrEmpty(studentSpecificId))
            {
                // Department
                IDepartmentService departmentService = _serviceProvider.GetService<IDepartmentService>();
                DepartmentList = (await departmentService.GetAllDepartments()).ToObservableCollection();
                DepartmentList.Insert(0, new() { Id = 0, DepartmentName = "- Select department -" });

                SelectedDepartment = DepartmentList[0];

                // Branch
                BranchList = new() { new() { BranchName = "- Select branch -" } };

                SelectedBranch = BranchList[0];

                // Province
                IProvinceService provinceService = _serviceProvider.GetService<IProvinceService>();
                ProvinceList = (await provinceService.GetAllProvinces()).ToObservableCollection();
                ProvinceList.Insert(0, new() { Id = 0, ProvinceName = "- Select province -" });

                SelectedProvince = ProvinceList[0];

                // District
                DistrictList = new() { new() { DistrictName = "- Select district -" } };

                SelectedDistrict = DistrictList[0];

                // Priority Types
                IPriorityTypeService priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
                primaryPriorityTypeList = (await priorityTypeService.GetAllPriorityTypesAsync()).ToList();

                PriorityType nonePriorityType = primaryPriorityTypeList.First(pt => pt.Id == 1);
                SelectedPriorityTypeList = new() { new() { PriorityName = nonePriorityType.PriorityName } };

                PriorityTypeList = primaryPriorityTypeList
                    .Where(pt => pt.Id != 1 && pt.Id != 2)
                    .ToObservableCollection();
                PriorityTypeList.Insert(0, new() { Id = 0, PriorityName = "- Select priority type -" });

                SelectedPriorityType = PriorityTypeList[0];
            }
            // Update student
            else
            {
                IStudentService studentService = _serviceProvider.GetService<IStudentService>();
                Student student = await studentService.GetFullInformationOfStudentBySpecificId(studentSpecificId);

                ImageUrl = student.ImageUrl;
                StudentSpecificId = student.StudentSpecificId;
                StudentName = student.FullName;
                Email = student.Email;
                DateOfBirth = student.DateOfBirth;

                // DepartmentList
                DepartmentList.Clear();

                IDepartmentService departmentService = _serviceProvider.GetService<IDepartmentService>();
                DepartmentList = (await departmentService.GetAllDepartments()).ToObservableCollection();
                DepartmentList.Insert(0, new() { Id = 0, DepartmentName = "- Select department -" });

                // SelectedDepartment
                SelectedDepartment = DepartmentList.First(d => d.Id == student.Branch.DepartmentId);

                // Branch
                BranchList.Clear();

                IBranchService branchService = _serviceProvider.GetService<IBranchService>();
                BranchList = (await branchService.GetBranchesByDepartmentId(student.Branch.DepartmentId)).ToObservableCollection();
                BranchList.Insert(0, new() { Id = 0, BranchName = "- Select branch -" });

                // SelectedBranch
                SelectedBranch = BranchList.First(b => b.Id == student.BranchId);

                // Province
                ProvinceList.Clear();
                
                IProvinceService provinceService = _serviceProvider.GetService<IProvinceService>();
                ProvinceList = (await provinceService.GetAllProvinces()).ToObservableCollection();
                ProvinceList.Insert(0, new() { Id = 0, ProvinceName = "- Select province -" });

                // SelectedProvince
                SelectedProvince = ProvinceList.First(p => p.Id == student.District.Province.Id);

                // District
                DistrictList.Clear();

                IDistrictService districtService = _serviceProvider.GetService<IDistrictService>();
                DistrictList = (await districtService.GetDistrictsByProvinceId(student.District.ProvinceId)).ToObservableCollection();
                DistrictList.Insert(0, new() { Id = 0, DistrictName = "- Select district -" });

                // SelectedDistrict
                SelectedDistrict = DistrictList.First(d => d.Id == student.DistrictId);

                // PrimaryPriorityTypeList
                IPriorityTypeService priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
                primaryPriorityTypeList = (await priorityTypeService.GetAllPriorityTypesAsync()).ToList();

                // SelectedPriorityTypeList
                SelectedPriorityTypeList.Clear();

                var selectedPriorityTypeListFromDatabase = (await priorityTypeService.GetPriorityTypesFromStudentIdAsync(student.Id)).ToObservableCollection();
                foreach (var priorityType in selectedPriorityTypeListFromDatabase)
                {
                    if (priorityType.Id != 2)
                    {
                        SelectedPriorityTypeList.Add(new() { PriorityName = priorityType.PriorityName });
                    }
                }

                if (SelectedPriorityTypeList.Count <= 0) 
                {
                    PriorityType nonePriorityType = primaryPriorityTypeList.First(pt => pt.Id == 1);
                    SelectedPriorityTypeList.Add(new() { PriorityName = nonePriorityType.PriorityName });
                }

                PriorityTypeList = primaryPriorityTypeList
                    .Where(pt => 
                        pt.Id != 1 && 
                        pt.Id != 2 &&
                        !selectedPriorityTypeListFromDatabase.Any(spt => spt.Id == pt.Id))
                    .ToObservableCollection();
                PriorityTypeList.Insert(0, new() { Id = 0, PriorityName = "- Select priority type -" });

                SelectedPriorityType = PriorityTypeList[0];
            }
        }

        [RelayCommand]
        public async Task ChooseImage()
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
                ImageUrl = result.FullPath;
            }
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
            //    Student student = students.Find(s => s.StudentSpecificId == StudentSpecificId);
            //    if (student != null)
            //    {
            //        IsLoading = false;
            //        await Application.Current.MainPage.DisplayAlert("Error", "Student has already existed!", "OK");
            //        return;
            //    }

            //    student = new Student
            //    {
            //        StudentSpecificId = StudentSpecificId,
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

        public bool CanAddStudentExecuted()
        {
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;

            //validate StudentSpecificId
            if (string.IsNullOrEmpty(StudentSpecificId))
            {
                if (globalVariable1 == 0)
                {
                    StudentSpecificIdColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    StudentSpecificIdColor = Color.FromArgb("#BF1D28");
                }
                StudentSpecificIdMessageText = "Student ID cannot be empty.";
                index1++;
            }
            else
            {
                globalVariable1 = 1;
                StudentSpecificIdColor = Color.FromArgb("#007D3A");
                StudentSpecificIdMessageText = "Valid student ID.";
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
            if (!ValidateEmail(Email))
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

        #region Property Changed
        async partial void OnSelectedDepartmentChanged(Department value)
        {
            if (value == null)
            {
                return;
            }

            if (value.DepartmentName == "- Select department -")
            {
                BranchList = new() { new() { BranchName = "- Select branch -" } };
            }
            else
            {
                IBranchService branchService = _serviceProvider.GetService<IBranchService>();
                BranchList = (await branchService.GetBranchesByDepartmentId(value.Id)).ToObservableCollection();
                BranchList.Insert(0, new() { Id = 0, BranchName = "- Select branch -" });
            }

            SelectedBranch = BranchList[0];
        }

        async partial void OnSelectedProvinceChanged(Province value)
        {
            if (value == null)
            {
                return;
            }

            if (value.ProvinceName == "- Select province -")
            {
                DistrictList = new() { new() { DistrictName = "- Select district -" } };
            }
            else
            {
                IDistrictService districtService = _serviceProvider.GetService<IDistrictService>();
                DistrictList = (await districtService.GetDistrictsByProvinceId(value.Id)).ToObservableCollection();
                DistrictList.Insert(0, new() { Id = 0, DistrictName = "- Select district -" });
            }

            SelectedDistrict = DistrictList[0];
        }
        #endregion

        #region Helper
        public void Clear()
        {
            StudentSpecificId = "";
            StudentName = "";
            Email = "";
            SelectedGender = "Male";
            ImageUrl = "https://static.wixstatic.com/media/8027bc_6d79e9c44bae49de97c018a781738884~mv2.jpg/v1/fill/w_987,h_1096,al_c,q_90/file.jpg";
            DateOfBirth = DateTime.Now;
        }

        private bool ValidateEmail(string email)
        {
            if (email == null)
            {
                return false;
            }

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
