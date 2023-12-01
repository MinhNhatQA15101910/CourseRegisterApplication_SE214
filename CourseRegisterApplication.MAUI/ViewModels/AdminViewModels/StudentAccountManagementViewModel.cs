using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    #region Displays
    public partial class StudentDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        private string studentSpecificId;

        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private DateTime dateOfBirth;

        [ObservableProperty]
        private Gender gender;

        [ObservableProperty]
        private bool activateStatus;

        [ObservableProperty]
        private Branch branch;

        [ObservableProperty]
        private Color backgroundColor;

        [ObservableProperty]
        private string avatar;

        public IStudentRequester StudentRequester { get; set; }
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseStudentAccount()
        {
            StudentRequester.ResetItemBackgrounds();
            BackgroundColor = Color.FromArgb("#1E90FF");

            StudentRequester.DisplayStudentAccountInformation(new Student
            {
                FullName = FullName,
                StudentSpecificId = StudentSpecificId,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Branch = Branch
            });
        }
        #endregion
    }
    #endregion

    #region IRequesters
    public interface IStudentRequester
    {
        void DisplayStudentAccountInformation(Student student);
        void ResetItemBackgrounds();
    }
    #endregion

    #region Main ViewModel
    public partial class StudentAccountManagementViewModel : ObservableObject, IStudentRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        #endregion

        #region Properties
        private List<StudentDisplay> primaryStudentAccountList = new();

        [ObservableProperty]
        private ObservableCollection<StudentDisplay> studentAccountList = new();

        public ObservableCollection<string> FilterOptions { get; set; } = new() { "Name", "Student ID" };

        [ObservableProperty]
        private string avatarUri = "blank_avatar.jpg";

        [ObservableProperty]
        private string fullName = "";

        [ObservableProperty]
        private string studentSpecificId = "";

        [ObservableProperty]
        private bool activateStatus;

        [ObservableProperty]
        private DateTime dateOfBirth;

        [ObservableProperty]
        private Gender gender;

        [ObservableProperty]
        private Branch branch;

        [ObservableProperty]
        private string selectedFilterOption = "Name";

        [ObservableProperty]
        private string searchFilter = "";

        #endregion

        #region Constructor
        public StudentAccountManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _studentService = serviceProvider.GetService<IStudentService>();
            _userService = serviceProvider.GetService<IUserService>();
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
        public async Task GetStudentAccount()
        {
            var studentList = await _studentService.GetStudents();
            ReloadStudentAccountList(studentList);
        }
       
        #endregion

        #region Implement IStudentRequester
        public void ResetItemBackgrounds()
        {
            foreach (var account in StudentAccountList)
            {
                account.BackgroundColor = Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayStudentAccountInformation(Student student)
        {
            AvatarUri = "profile_avatar";
            FullName = student.FullName;
            StudentSpecificId = student.StudentSpecificId;
            ActivateStatus = IsStudentHasAccount(student);
            Gender = student.Gender;
            DateOfBirth = student.DateOfBirth;
            Branch = student.Branch;
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                SearchFilter = "";
                ResetItemBackgrounds();
                ResetAccountInformation();

                switch (newValue)
                {
                    case "Full Name":
                        StudentAccountList = primaryStudentAccountList.OrderBy(a => a.FullName).ToObservableCollection();
                        break;
                    case "Student ID":
                        StudentAccountList = primaryStudentAccountList.OrderBy(a => a.StudentSpecificId).ToObservableCollection();
                        break;
                }
            }
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            ResetItemBackgrounds();
            ResetAccountInformation();

            switch (SelectedFilterOption)
            {
                case "Name":
                    StudentAccountList = primaryStudentAccountList.Where(a => a.FullName.Contains(newValue)).OrderBy(a => a.FullName).ToObservableCollection();
                    break;
                case "Student ID":
                    StudentAccountList = primaryStudentAccountList.Where(a => a.StudentSpecificId.Contains(newValue)).OrderBy(a => a.StudentSpecificId).ToObservableCollection();
                    break;
            }
        }
        #endregion

        #region Helpers
        private void ReloadStudentAccountList(List<Student> accountList)
        {
            primaryStudentAccountList.Clear();

            if (accountList.Count > 0)
            {
                foreach (var account in accountList)
                {
                    primaryStudentAccountList.Add(new StudentDisplay
                    {
                        FullName = account.FullName,
                        StudentSpecificId = account.StudentSpecificId,
                        DateOfBirth = account.DateOfBirth,
                        Avatar = "profile_avatar.png",
                        BackgroundColor = Color.FromArgb("#EBF6FF"),
                        StudentRequester = this,
                    });
                }

                StudentAccountList = primaryStudentAccountList.OrderBy(a => a.FullName).ToObservableCollection();
            }
        }

        private void ResetAccountInformation()
        {
            AvatarUri = "blank_avatar.jpg";
            FullName = "";
            StudentSpecificId = "";
            DateOfBirth = DateTime.Today;
            string formattedDate = DateOfBirth.ToString("dd/MM/yyyy");
        }

        private bool IsStudentHasAccount(Student student)
        {
            var status = _userService.GetStudentUsers().Result.Any(u => u.Username == student.StudentSpecificId);
            return status;
        }
        #endregion
    }
    #endregion
}
