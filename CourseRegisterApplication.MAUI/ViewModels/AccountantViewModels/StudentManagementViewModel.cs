using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    #region Displays
    public partial class StudentDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty] private string studentSpecificId;

        [ObservableProperty] private string fullName;

        [ObservableProperty] private DateTime dateOfBirth;

        [ObservableProperty] private Gender gender;

        [ObservableProperty] private string activateStatus;

        [ObservableProperty] private string email;

        [ObservableProperty] private Branch branch;

        [ObservableProperty] private Department department;

        [ObservableProperty] private string address;

        [ObservableProperty] private Color backgroundColor;

        [ObservableProperty] private Color statusColor;

        [ObservableProperty] private string avatar;

        public bool PrimaryStatus { get; set; }

        public IStudentRequester StudentRequester { get; set; }
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseStudent()
        {
            StudentRequester.ResetItemBackgrounds();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            StudentRequester.DisplayStudentInformation(this);
        }
        #endregion
    }
    #endregion

    #region IRequesters
    public interface IStudentRequester
    {
        void DisplayStudentInformation(StudentDisplay studentDisplay);
        void ResetItemBackgrounds();
    }
    #endregion

    #region Main ViewModel
    public partial class StudentManagementViewModel : ObservableObject, IStudentRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        private readonly List<StudentDisplay> primaryStudentList = new();

        [ObservableProperty] private ObservableCollection<StudentDisplay> studentList = new();

        public ObservableCollection<string> FilterOptions { get; set; } = new() { "Name", "Student ID" };

        [ObservableProperty] private string avatarUri = "blank_avatar.jpg";

        [ObservableProperty] private string fullName;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(NavigateToUpdateStudentPageCommand))]
        private string studentSpecificId;

        [ObservableProperty] private string activateStatus;

        [ObservableProperty] private string dateOfBirth;

        [ObservableProperty] private string email;

        [ObservableProperty] private string gender;

        [ObservableProperty] private string branchName;

        [ObservableProperty] private string departmentName;

        [ObservableProperty] private string district;

        [ObservableProperty] private string province;

        [ObservableProperty] private string address;

        [ObservableProperty] private string age;

        [ObservableProperty] private string selectedFilterOption = "Name";

        [ObservableProperty] private string searchFilter;

        #endregion

        #region Constructor
        public StudentManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
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
        public async Task NavigateToAddStudentPage()
        {
            AddUpdateStudentViewModel addUpdateStudentViewModel = _serviceProvider.GetService<AddUpdateStudentViewModel>();
            addUpdateStudentViewModel.GetInformationCommand.Execute(null);

            await Shell.Current.GoToAsync(nameof(AddUpdateStudentPage), true);
        }

        [RelayCommand(CanExecute = nameof(CanNavigateToUpdateStudentPageCommandExecuted))]
        public async Task NavigateToUpdateStudentPage()
        {
            AddUpdateStudentViewModel addUpdateStudentViewModel = _serviceProvider.GetService<AddUpdateStudentViewModel>();
            addUpdateStudentViewModel.GetInformationCommand.Execute(StudentSpecificId);

            await Shell.Current.GoToAsync(nameof(AddUpdateStudentPage), true);
        }

        public bool CanNavigateToUpdateStudentPageCommandExecuted()
        {
            return !string.IsNullOrEmpty(StudentSpecificId);
        }

        [RelayCommand]
        public async Task GetStudents()
        {
            IStudentService studentService = _serviceProvider.GetService<IStudentService>();
            List<Student> students = await studentService.GetFullInformationOfAllStudents();

            await ReloadStudentList(students);
        }

        #endregion

        #region Implement IStudentRequester
        public void ResetItemBackgrounds()
        {
            for (int i = 0; i < StudentList.Count; i++)
            {
                StudentList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayStudentInformation(StudentDisplay studentDisplay)
        {
            AvatarUri = studentDisplay.Avatar;
            FullName = studentDisplay.FullName;
            StudentSpecificId = studentDisplay.StudentSpecificId;
            Email = studentDisplay.Email;
            Gender = studentDisplay.Gender.ToString();
            DateOfBirth = studentDisplay.DateOfBirth.ToString("dd/MM/yyyy");
            BranchName = studentDisplay.Branch.BranchName;
            DepartmentName = studentDisplay.Department.DepartmentName;
            Address = studentDisplay.Address;
            Age = Helpers.CalculateAge(studentDisplay.DateOfBirth).ToString();
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string value)
        {
            switch (value)
            {
                case "Name":
                    StudentList = primaryStudentList.OrderBy(a => a.FullName).ToObservableCollection();
                    break;
                case "Student ID":
                    StudentList = primaryStudentList.OrderBy(a => a.StudentSpecificId).ToObservableCollection();
                    break;
            }

            SearchFilter = "";
            ResetItemBackgrounds();
            ResetStudentInformation();
        }

        partial void OnSearchFilterChanged(string value)
        {
            switch (SelectedFilterOption)
            {
                case "Name":
                    StudentList = primaryStudentList
                        .Where(
                            a => a.FullName
                                .ToLower()
                                .Contains(value.ToLower()))
                        .OrderBy(a => a.FullName)
                        .ToObservableCollection();
                    break;
                case "Student ID":
                    StudentList = primaryStudentList
                        .Where(
                            a => a.StudentSpecificId
                                .ToLower()
                                .Contains(value.ToLower()))
                        .OrderBy(a => a.StudentSpecificId)
                        .ToObservableCollection();
                    break;
            }

            ResetItemBackgrounds();
            ResetStudentInformation();

        }
        #endregion

        #region Helpers
        private async Task ReloadStudentList(List<Student> studentList)
        {
            primaryStudentList.Clear();
            StudentList.Clear();

            if (studentList.Count > 0)
            {
                foreach (var student in studentList)
                {
                    primaryStudentList.Add(new StudentDisplay
                    {
                        FullName = student.FullName,
                        StudentSpecificId = student.StudentSpecificId,
                        Email = student.Email,
                        Gender = student.Gender,
                        DateOfBirth = student.DateOfBirth,
                        Branch = student.Branch,
                        Department = student.Branch.Department,
                        Address = $"{student.District.DistrictName}, {student.District.Province.ProvinceName}",
                        Avatar = student.ImageUrl,
                        ActivateStatus = await IsStudentHasAccount(student) ? "Activated" : "Pending",
                        StatusColor = await IsStudentHasAccount(student) ? Color.FromArgb("#007D3A") : Color.FromArgb("#CC8100"),
                        BackgroundColor = Color.FromArgb("#EBF6FF"),
                        StudentRequester = this,
                    });
                }

                StudentList = primaryStudentList.OrderBy(a => a.FullName).ToObservableCollection();

                ResetItemBackgrounds();
            }
        }

        public void ResetStudentInformation()
        {
            AvatarUri = "blank_avatar.jpg";
            FullName = "";
            StudentSpecificId = "";
            Email = "";
            Gender = "";
            DepartmentName = "";
            Address = "";
            Age = "";
            DateOfBirth = null;
            BranchName = "";
        }

        private async Task<bool> IsStudentHasAccount(Student student)
        {
            IUserService userService = _serviceProvider.GetService<IUserService>();
            User studentUser = await userService.GetUserByUsername(student.StudentSpecificId);
            
            return studentUser != null;
        }
        #endregion
    }
    #endregion
}
