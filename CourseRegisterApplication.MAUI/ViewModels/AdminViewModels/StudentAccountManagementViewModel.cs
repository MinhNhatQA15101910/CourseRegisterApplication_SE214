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
        private string email;

        [ObservableProperty]
        private string branchName;

        [ObservableProperty]
        private string departmentName;

        [ObservableProperty]
        private Color backgroundColor;

        [ObservableProperty]
        private Color checkBoxColor;

        [ObservableProperty]
        private string avatar;

        public bool PrimaryStatus { get; set; }

        public IStudentRequester StudentRequester { get; set; }
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseStudentAccount()
        {
            StudentRequester.ResetItemBackgrounds();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            StudentRequester.DisplayStudentAccountInformation(this);
        }
        #endregion

        partial void OnActivateStatusChanged(bool oldValue, bool newValue)
        {
            if(newValue != PrimaryStatus)
            {
                CheckBoxColor = Color.FromArgb("#DE3226");
            }
            else
            {
                CheckBoxColor = Color.FromArgb("#3189CC");
            }

            if (StudentRequester != null)
            {
                StudentRequester.NotifyCanSaveChanges();
            }
        }

    }
    #endregion

    #region IRequesters
    public interface IStudentRequester
    {
        void DisplayStudentAccountInformation(StudentDisplay studentDisplay);
        void ResetItemBackgrounds();
        void NotifyCanSaveChanges();
    }
    #endregion

    #region Main ViewModel
    public partial class StudentAccountManagementViewModel : ObservableObject, IStudentRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        private readonly List<StudentDisplay> originalPrimaryStudentDisplayList = new();

        private List<StudentDisplay> primaryStudentAccountList = new();

        [ObservableProperty]
        private ObservableCollection<StudentDisplay> studentAccountList = new();

        [ObservableProperty]
        private string avatarUri = "blank_avatar.jpg";

        [ObservableProperty]
        private string fullName = "";

        [ObservableProperty]
        private string studentSpecificId = "";

        [ObservableProperty]
        private bool activateStatus;

        [ObservableProperty]
        private string dateOfBirth;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private Gender gender;

        [ObservableProperty]
        private string branch;

        [ObservableProperty]
        private string department;

        [ObservableProperty]
        private string searchFilter = "";

        #endregion

        #region Constructor
        public StudentAccountManagementViewModel(IServiceProvider serviceProvider)
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
        public async Task OpenFilterPopup()
        {
            var filterStudentAccountPopup = _serviceProvider.GetService<FilterStudentAccountPopup>();
            await Application.Current.MainPage.ShowPopupAsync(filterStudentAccountPopup);
        }

        [RelayCommand]
        public async Task GetStudentAccounts()
        {
            IStudentService studentService = _serviceProvider.GetService<IStudentService>();
            List<Student> studentList = await studentService.GetAllStudents();
            
            await ReloadStudentAccountList(studentList);
        }

        [RelayCommand]
        public async Task Cancel()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to reverse changes?", "Yes", "No");
            if (result)
            {
                GetStudentAccountsCommand.Execute(null);
            }
        }

        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        public async Task SaveChanges()
        {
            // Filter accounts need to be updated
            IUserService userService = _serviceProvider.GetService<IUserService>();
            List<User> studentUserList = await userService.GetStudentUsers();
            List<StudentDisplay> accountsNeedToBeUpdated = StudentAccountList.Where(a => a.ActivateStatus != a.PrimaryStatus).ToList();
            bool accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to change Active Status for these students?", "Yes", "No");
            if (accept)
            {
                foreach (var account in accountsNeedToBeUpdated)
                {
                    if (account.ActivateStatus)
                    {
                        // Create new user
                        User newUser = new()
                        {
                            Username = account.StudentSpecificId,
                            //Password = Helpers.EncryptData(Helpers.GeneratePassword()),
                            Password = "12345678",
                            Email = account.Email,
                            Role = Role.Student,
                        };

                        // Add new user to database
                        User user = await userService.AddUser(newUser);
                        if(user == null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Error occurred!", "OK");
                            return;
                        }
                    }
                    else
                    {
                        // Delete user from database
                        User deleteUser = studentUserList.Find(u => u.Username == account.StudentSpecificId);
                        await userService.DeleteUser(deleteUser.Id);
                    }
                }
                await Application.Current.MainPage.DisplayAlert("Success", "Changes have been saved", "OK");

                GetStudentAccountsCommand.Execute(null);
            }
        }

        public bool CanSaveChanges()
        {
            var account = primaryStudentAccountList.Find(a => a.PrimaryStatus != a.ActivateStatus);

            return account != null;
        }

        #endregion

        #region Implement IStudentRequester
        public void ResetItemBackgrounds()
        {
            for (int i = 0; i < StudentAccountList.Count; i++)
            {
                StudentAccountList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayStudentAccountInformation(StudentDisplay studentDisplay)
        {
            AvatarUri = studentDisplay.Avatar;
            FullName = studentDisplay.FullName;
            StudentSpecificId = studentDisplay.StudentSpecificId;
            Email = studentDisplay.Email;
            Gender = studentDisplay.Gender;
            DateOfBirth = studentDisplay.DateOfBirth.ToString("dd/MM/yyyy");
            Branch = studentDisplay.BranchName;
            Department = studentDisplay.DepartmentName;
        }
        #endregion

        #region Helpers
        private async Task ReloadStudentAccountList(List<Student> accountList)
        {
            originalPrimaryStudentDisplayList.Clear();

            if (accountList.Count > 0)
            {
                IBranchService branchService = _serviceProvider.GetService<IBranchService>();
                IDepartmentService departmentService = _serviceProvider.GetService<IDepartmentService>();

                foreach (var account in accountList)
                {
                    Branch accountBranch = await branchService.GetBranchById(account.BranchId);
                    Department accountDepartment = await departmentService.GetDepartmentById(accountBranch.DepartmentId);

                    originalPrimaryStudentDisplayList.Add(new StudentDisplay
                    {
                        FullName = account.FullName,
                        StudentSpecificId = account.StudentSpecificId,
                        Email = account.Email,
                        Gender = account.Gender,
                        DateOfBirth = account.DateOfBirth,
                        BranchName = accountBranch.BranchName,
                        DepartmentName = accountDepartment.DepartmentName,
                        Avatar = account.ImageUrl,
                        ActivateStatus = await IsStudentHasAccount(account),
                        PrimaryStatus = await IsStudentHasAccount(account),
                        CheckBoxColor = Color.FromArgb("#3189CC"),
                        BackgroundColor = Color.FromArgb("#EBF6FF"),
                        StudentRequester = this,
                    });
                }

                primaryStudentAccountList = originalPrimaryStudentDisplayList;
                StudentAccountList = primaryStudentAccountList.OrderBy(a => a.FullName).ToObservableCollection();
                for (int i = 0; i < StudentAccountList.Count; i++)
                {
                    StudentAccountList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
                }
            }
        }

        public void ResetAccountInformation()
        {
            AvatarUri = "blank_avatar.jpg";
            FullName = "";
            StudentSpecificId = "";
            Email = "";
            Department = "";
            DateOfBirth = null;
            Branch = null;
        }

        private async Task<bool> IsStudentHasAccount(Student student)
        {
            IUserService userService = _serviceProvider.GetService<IUserService>();
            List<User> userList = await userService.GetStudentUsers();
            foreach (var user in userList)
            {
                if (user.Username == student.StudentSpecificId)
                {
                    ActivateStatus = true;
                    return true;
                }
            }
            return false;
        }

        public void NotifyCanSaveChanges()
        {
            SaveChangesCommand.NotifyCanExecuteChanged();
        }

        #endregion
    }
    #endregion
}
