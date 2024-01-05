using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    #region Displays
    public partial class SelectedPriorityTypeDisplay : ObservableObject
    {
        public ISelectedPriorityTypeRequester SelectedPriorityTypeRequester { get; set; }

        public int Id { get; set; }

        [ObservableProperty] private string priorityName;

        [RelayCommand]
        public void UnselectPriorityType()
        {
            if (Id != 1)
            {
                SelectedPriorityTypeRequester.UnselectPriorityType(this);
            }
        }
    }
    #endregion

    #region Requesters
    public interface ISelectedPriorityTypeRequester
    {
        void UnselectPriorityType(SelectedPriorityTypeDisplay selectedPriorityTypeDisplay);
    }
    #endregion

    #region Main ViewModel
    public partial class AddUpdateStudentViewModel : ObservableObject, ISelectedPriorityTypeRequester
    {
        #region Service
        private readonly IServiceProvider _serviceProvider;
        private readonly FirebaseStorage _firebaseStorage = new("courseregistrationfirebase.appspot.com");
        private FileResult imageResult;
        #endregion

        #region Properties
        public string CommandName { get; set; }

        public int Id { get; set; } = -1;

        public string OldImageUrl { get; set; }

        [ObservableProperty] private string imageUrl = "https://static.wixstatic.com/media/8027bc_6d79e9c44bae49de97c018a781738884~mv2.jpg/v1/fill/w_987,h_1096,al_c,q_90/file.jpg";

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateStudentCommand))]
        private string studentSpecificId;

        [ObservableProperty] private string studentSpecificIdMessageText;

        [ObservableProperty] private Color studentSpecificIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateStudentCommand))]
        private string studentName;

        [ObservableProperty] private string studentNameMessageText;

        [ObservableProperty] private Color studentNameColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateStudentCommand))]
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

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DisplayAddDistrictPopupCommand))]
        private Province selectedProvince;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateStudentCommand))]
        private District selectedDistrict;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateStudentCommand))]
        private Branch selectedBranch;

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

        #region Commands
        [RelayCommand]
        public async Task GetInformation(string studentSpecificId)
        {
            // Add student
            if (string.IsNullOrEmpty(studentSpecificId))
            {
                CommandName = "Add student";

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
                SelectedPriorityTypeList = new() { new() { SelectedPriorityTypeRequester = this, Id = nonePriorityType.Id, PriorityName = nonePriorityType.PriorityName } };

                PriorityTypeList = primaryPriorityTypeList
                    .Where(pt => pt.Id != 1 && pt.Id != 2)
                    .ToObservableCollection();
                PriorityTypeList.Insert(0, new() { Id = 0, PriorityName = "- Select priority type -" });

                SelectedPriorityType = PriorityTypeList[0];
            }
            // Update student
            else
            {
                CommandName = "Update student";

                IStudentService studentService = _serviceProvider.GetService<IStudentService>();
                Student student = await studentService.GetFullInformationOfStudentBySpecificId(studentSpecificId);

                Id = student.Id;
                OldImageUrl = student.ImageUrl;

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
                        SelectedPriorityTypeList.Add(new() { SelectedPriorityTypeRequester = this, Id = priorityType.Id, PriorityName = priorityType.PriorityName });
                    }
                }

                if (SelectedPriorityTypeList.Count <= 0)
                {
                    PriorityType nonePriorityType = primaryPriorityTypeList.First(pt => pt.Id == 1);
                    SelectedPriorityTypeList.Add(new() { SelectedPriorityTypeRequester = this, Id = nonePriorityType.Id, PriorityName = nonePriorityType.PriorityName });
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
            imageResult = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Please select an image file.",
                FileTypes = FilePickerFileType.Images
            });

            if (imageResult != null &&
                (imageResult.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                imageResult.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                imageResult.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
                imageResult.FileName.EndsWith("svg", StringComparison.OrdinalIgnoreCase) ||
                imageResult.FileName.EndsWith("gif", StringComparison.OrdinalIgnoreCase)))
            {
                ImageUrl = imageResult.FullPath;
            }
        }

        [RelayCommand]
        public void SelectPriorityType()
        {
            if (SelectedPriorityType.PriorityName != "- Select priority type -")
            {
                foreach (var priorityType in SelectedPriorityTypeList)
                {
                    if (priorityType.Id == 1)
                    {
                        SelectedPriorityTypeList.Remove(priorityType);
                        break;
                    }
                }

                SelectedPriorityTypeList.Add(new() { SelectedPriorityTypeRequester = this, Id = SelectedPriorityType.Id, PriorityName = SelectedPriorityType.PriorityName });

                PriorityTypeList.Remove(SelectedPriorityType);

                SelectedPriorityType = PriorityTypeList[0];
            }
        }

        [RelayCommand]
        public async Task DisplayAddProvincePopup()
        {
            AddUpdateProvincePopup addUpdateProvincePopup = _serviceProvider.GetService<AddUpdateProvincePopup>();

            AddUpdateProvinceViewModel addUpdateProvinceViewModel = _serviceProvider.GetService<AddUpdateProvinceViewModel>();
            addUpdateProvinceViewModel.CommandName = "Add province";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateProvincePopup);
        }

        [RelayCommand(CanExecute = nameof(CanDisplayAddDistrictPopupCommandExecuted))]
        public async Task DisplayAddDistrictPopup()
        {
            var addUpdateDistrictPopup = _serviceProvider.GetService<AddUpdateDistrictPopup>();
            var addUpdateDistrictViewModel = _serviceProvider.GetService<AddUpdateDistrictViewModel>();

            addUpdateDistrictViewModel.CommandName = "Add district";
            addUpdateDistrictViewModel.ProvinceId = SelectedProvince.Id;
            addUpdateDistrictViewModel.ProvinceName = SelectedProvince.ProvinceName;

            await Application.Current.MainPage.ShowPopupAsync(addUpdateDistrictPopup);
        }

        [RelayCommand]
        public async Task DisplayAddDepartmentPopup()
        {
            AddUpdateDepartmentPopup addUpdateDepartmentPopup = _serviceProvider.GetService<AddUpdateDepartmentPopup>();
            AddUpdateDepartmentViewModel addUpdateDepartmentViewModel = _serviceProvider.GetService<AddUpdateDepartmentViewModel>();

            addUpdateDepartmentViewModel.CommandName = "Add department";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateDepartmentPopup);
        }

        [RelayCommand]
        public async Task DisplayAddBranchPopup()
        {
            AddUpdateBranchPopup addUpdateBranchPopup = _serviceProvider.GetService<AddUpdateBranchPopup>();
            AddUpdateBranchViewModel addUpdateBranchViewModel = _serviceProvider.GetService<AddUpdateBranchViewModel>();

            addUpdateBranchViewModel.CommandName = "Add branch";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateBranchPopup);
        }

        [RelayCommand]
        public async Task DisplayAddPriorityTypePopup()
        {
            await Application.Current.MainPage.DisplayAlert("Information", "This feature hasn't been developed yet!", "Ok");
        }

        private bool CanDisplayAddDistrictPopupCommandExecuted()
        {
            return (SelectedProvince != null) && (SelectedProvince.ProvinceName != "- Select province -");
        }

        [RelayCommand(CanExecute = nameof(CanAddStudentExecuted))]
        public async Task AddUpdateStudent()
        {
            if (CommandName == "Add student")
            {
                await AddStudent();
            }
            else if (CommandName == "Update student")
            {
                await UpdateStudent();
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
            Clear();

            await Shell.Current.GoToAsync("..", true);
        }

        public bool CanAddStudentExecuted()
        {
            bool isValidStudentSpecificId = true;
            bool isValidStudentName = true;
            bool isValidEmail = true;
            bool isValidBranch = (SelectedBranch != null) && (SelectedBranch.BranchName != "- Select branch -");
            bool isValidDistrict = (SelectedDistrict != null) && SelectedDistrict.DistrictName != "- Select district -";

            // Validate StudentSpecificId
            if (string.IsNullOrEmpty(StudentSpecificId))
            {
                StudentSpecificIdColor = Color.FromArgb("#BF1D28");
                StudentSpecificIdMessageText = "Student ID cannot be empty.";
                isValidStudentSpecificId = false;
            }
            else
            {
                StudentSpecificIdColor = Color.FromArgb("#007D3A");
                StudentSpecificIdMessageText = "Valid student ID.";
            }

            // Validate StudentName
            if (string.IsNullOrEmpty(StudentName))
            {
                StudentNameColor = Color.FromArgb("#BF1D28");
                StudentNameMessageText = "Student name cannot be empty.";
                isValidStudentName = false;
            }
            else
            {
                StudentNameColor = Color.FromArgb("#007D3A");
                StudentNameMessageText = "Valid student name.";
            }

            // Validate Email
            if (!ValidateEmail(Email))
            {
                EmailColor = Color.FromArgb("#BF1D28");
                EmailMessageText = "Invalid Email.";
                isValidEmail = false;
            }
            else
            {
                EmailColor = Color.FromArgb("#007D3A");
                EmailMessageText = "Valid email.";
            }

            return isValidStudentSpecificId && isValidStudentName && isValidEmail && isValidBranch && isValidDistrict;
        }
        #endregion

        #region Implement ISelectedPriorityTypeRequester
        public void UnselectPriorityType(SelectedPriorityTypeDisplay selectedPriorityTypeDisplay)
        {
            SelectedPriorityTypeList.Remove(selectedPriorityTypeDisplay);

            if (SelectedPriorityTypeList.Count == 0)
            {
                PriorityType nonePriorityType = primaryPriorityTypeList.First(pt => pt.Id == 1);
                SelectedPriorityTypeList.Add(new() { SelectedPriorityTypeRequester = this, Id = nonePriorityType.Id, PriorityName = nonePriorityType.PriorityName });
            }

            PriorityType priorityType = primaryPriorityTypeList.First(pt => pt.Id == selectedPriorityTypeDisplay.Id);
            PriorityTypeList.Add(priorityType);

            SelectedPriorityType = PriorityTypeList[0];
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
            CommandName = "";
            Id = -1;
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

        public async Task ReloadProvinceList()
        {
            IProvinceService provinceService = _serviceProvider.GetService<IProvinceService>();
            ProvinceList = (await provinceService.GetAllProvinces()).ToObservableCollection();
            ProvinceList.Insert(0, new() { Id = 0, ProvinceName = "- Select province -" });

            SelectedProvince = ProvinceList[0];
        }

        public async Task ReloadDistrictList()
        {
            IDistrictService districtService = _serviceProvider.GetService<IDistrictService>();
            DistrictList = (await districtService.GetDistrictsByProvinceId(SelectedProvince.Id)).ToObservableCollection();
            DistrictList.Insert(0, new() { Id = 0, DistrictName = "- Select district -" });

            SelectedDistrict = DistrictList[0];
        }

        public async Task ReloadDepartmentList()
        {
            IDepartmentService departmentService = _serviceProvider.GetService<IDepartmentService>();
            DepartmentList = (await departmentService.GetAllDepartments()).ToObservableCollection();
            DepartmentList.Insert(0, new() { Id = 0, DepartmentName = "- Select department -" });

            SelectedDepartment = DepartmentList[0];
        }

        public async Task ReloadBranchList()
        {
            IBranchService branchService = _serviceProvider.GetService<IBranchService>();
            BranchList = (await branchService.GetBranchesByDepartmentId(SelectedDepartment.Id)).ToObservableCollection();
            BranchList.Insert(0, new() { Id = 0, BranchName = "- Select branch -" });

            SelectedBranch = BranchList[0];
        }

        public async Task ReloadPriorityTypeList()
        {
            IPriorityTypeService priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
            primaryPriorityTypeList = (await priorityTypeService.GetAllPriorityTypesAsync()).ToList();

            PriorityTypeList = primaryPriorityTypeList
                    .Where(pt => pt.Id != 1 && pt.Id != 2)
                    .ToObservableCollection();
            PriorityTypeList.Insert(0, new() { Id = 0, PriorityName = "- Select priority type -" });

            SelectedPriorityType = PriorityTypeList[0];
        }

        private async Task AddStudent()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new student?", "Yes", "No");
            if (accept)
            {
                IStudentService studentService = _serviceProvider.GetService<IStudentService>();
                List<Student> studentList = await studentService.GetAllStudents();

                // Check if there is any student in the database with the same StudentSpecificId
                List<Student> sameSpecifcIdStudents = studentList
                    .Where(s => s.StudentSpecificId.ToLower() == StudentSpecificId.ToLower())
                    .ToList();
                if (sameSpecifcIdStudents.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this student because there is another student with the same Id!", "OK");
                    return;
                }

                // Add student
                string newImageUrl;
                if (ImageUrl != "https://static.wixstatic.com/media/8027bc_6d79e9c44bae49de97c018a781738884~mv2.jpg/v1/fill/w_987,h_1096,al_c,q_90/file.jpg")
                {
                    using var stream = await imageResult.OpenReadAsync();
                    newImageUrl = await _firebaseStorage
                        .Child("Images")
                        .Child("Avatar")
                        .Child($"{StudentSpecificId.Trim()}-Avatar")
                        .PutAsync(stream);
                }
                else
                {
                    newImageUrl = ImageUrl;
                }

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    // Update database
                    Student student = new()
                    {
                        StudentSpecificId = StudentSpecificId.Trim(),
                        FullName = StudentName.Trim(),
                        DateOfBirth = DateOfBirth,
                        Email = Email,
                        Gender = (SelectedGender == "Male") ? Gender.Male : Gender.Female,
                        BranchId = SelectedBranch.Id,
                        Branch = null,
                        DistrictId = SelectedDistrict.Id,
                        District = null,
                        ImageUrl = newImageUrl
                    };
                    List<PriorityType> priorityTypes = new();
                    foreach (var priorityType in SelectedPriorityTypeList)
                    {
                        priorityTypes.Add(primaryPriorityTypeList.First(pt => pt.Id == priorityType.Id));
                    }

                    Student result = await studentService.AddStudent(student, priorityTypes);
                    if (result != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Add student successfully!", "OK");

                        StudentManagementViewModel studentManagementViewModel = _serviceProvider.GetService<StudentManagementViewModel>();
                        await studentManagementViewModel.ReloadStudentListWhenAddNewStudent(await studentService.GetFullInformationOfStudentBySpecificId(StudentSpecificId));

                        NavigateBackCommand.Execute(null);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed", "Add student failed!", "OK");
                    }
                }
            }
        }

        private async Task UpdateStudent()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to update this student?", "Yes", "No");
            if (accept)
            {
                IStudentService studentService = _serviceProvider.GetService<IStudentService>();
                List<Student> studentList = await studentService.GetAllStudents();

                // Check if there is any student in the database with the same StudentSpecificId
                List<Student> sameSpecifcIdStudents = studentList
                    .Where(s => s.StudentSpecificId.ToLower() == StudentSpecificId.ToLower() && s.Id != Id)
                    .ToList();
                if (sameSpecifcIdStudents.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this student because there is another student with the same specific Id!", "OK");
                    return;
                }

                // Update student
                string newImageUrl;
                if (ImageUrl != OldImageUrl)
                {
                    using var stream = await imageResult.OpenReadAsync();
                    newImageUrl = await _firebaseStorage
                        .Child("Images")
                        .Child("Avatar")
                        .Child($"{StudentSpecificId.Trim()}-Avatar")
                        .PutAsync(stream);
                }
                else
                {
                    newImageUrl = ImageUrl;
                }

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    // Update database
                    Student student = new()
                    {
                        Id = Id,
                        StudentSpecificId = StudentSpecificId.Trim(),
                        FullName = StudentName.Trim(),
                        DateOfBirth = DateOfBirth,
                        Email = Email,
                        Gender = (SelectedGender == "Male") ? Gender.Male : Gender.Female,
                        BranchId = SelectedBranch.Id,
                        Branch = null,
                        DistrictId = SelectedDistrict.Id,
                        District = null,
                        ImageUrl = newImageUrl
                    };
                    List<PriorityType> priorityTypes = new();
                    foreach (var priorityType in SelectedPriorityTypeList)
                    {
                        priorityTypes.Add(primaryPriorityTypeList.First(pt => pt.Id == priorityType.Id));
                    }

                    bool result = await studentService.UpdateStudent(Id, student, priorityTypes);
                    if (result)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Update student successfully!", "OK");

                        StudentManagementViewModel studentManagementViewModel = _serviceProvider.GetService<StudentManagementViewModel>();
                        await studentManagementViewModel.ReloadStudentListWhenUpdateStudent(await studentService.GetFullInformationOfStudentBySpecificId(StudentSpecificId));

                        NavigateBackCommand.Execute(null);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed", "Update student failed!", "OK");
                    }
                }
            }
        }
        #endregion
    }

    #endregion
}
