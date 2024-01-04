using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    #region Displays
    public partial class StudentDisplay : ObservableObject
    {
        #region Properties
        public int Index { get; set; }

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
        private Branch branch;

        [ObservableProperty]
        private string department;

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

            StudentRequester.DisplayStudentAccountInformation(new Student
            {
                FullName = FullName,
                StudentSpecificId = StudentSpecificId,
                Gender = Gender,
                Branch = Branch,
                DateOfBirth = DateOfBirth,
            });
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
        void DisplayStudentAccountInformation(Student student);
        void ResetItemBackgrounds();
        void NotifyCanSaveChanges();
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
        private string dateOfBirth;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string gender;

        [ObservableProperty]
        private string branch;

        [ObservableProperty]
        private string department;

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
        public void GetStudentAccount()
        {
            //var studentList = await _studentService.GetStudents();

            List<Student> studentList = new()
            {
                new()
                {
                    FullName = "Trương Bá Cường",
                    StudentSpecificId = "SV21520013",
                    Gender = Shared.Gender.Male,
                    Branch = new Branch
                    {
                        BranchName = "Kỹ thuật phần mềm",
                        BranchSpecificId = "KTPM",
                        Department = new Department
                        {
                            DepartmentName = "Công nghệ phần mềm",
                            DepartmentSpecificId = "CNPM",
                        }
                    },
                },
                new()
                {
                    FullName = "Đôn Khánh Duy",
                    StudentSpecificId = "SV21520032",
                    Gender = Shared.Gender.Male,
                    Branch = new Branch
                    {
                        BranchName = "Công nghệ thông tin",
                        BranchSpecificId = "CNTT",
                        Department = new Department
                        {
                            DepartmentName = "Công nghệ thông tin",
                            DepartmentSpecificId = "CNTT",
                        }
                    },
                },new()
                {
                    FullName = "Nguyễn Thị Phương",
                    StudentSpecificId = "SV21520135",
                    Gender = Shared.Gender.Female,
                    Branch = new Branch
                    {
                        BranchName = "Công nghệ thông tin",
                        BranchSpecificId = "CNTT",
                        Department = new Department
                        {
                            DepartmentName = "Công nghệ thông tin",
                            DepartmentSpecificId = "CNTT",
                        }
                    },
                },new()
                {
                    FullName = "Huỳnh Bá Anh Quân",
                    StudentSpecificId = "SV21520136",
                    DateOfBirth = new DateTime(2003, 05, 15),
                    Gender = Shared.Gender.Male,
                    Branch = new Branch
                    {
                        BranchName = "Kỹ thuật phần mềm",
                        BranchSpecificId = "KTPM",
                        Department = new Department
                        {
                            DepartmentName = "Công nghệ phần mềm",
                            DepartmentSpecificId = "CNPM",
                        }
                    },
                },new()
                {
                    FullName = "Võ Thanh Bình",
                    StudentSpecificId = "SV21520007",
                    DateOfBirth = new DateTime(2003, 02, 10),
                    Gender = Shared.Gender.Male,
                    Branch = new Branch
                    {
                        BranchName = "Khoa học máy tính",
                        BranchSpecificId = "KHMT",
                        Department = new Department
                        {
                            DepartmentName = "Khoa học máy tính",
                            DepartmentSpecificId = "KHMT",
                        }
                    },
                },
            };
            
            ReloadStudentAccountList(studentList);
        }

        [RelayCommand]
        public async Task Cancel()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to reverse changes?", "Yes", "No");
            if (result)
            {
                getStudentAccountCommand.Execute(null);
            }
        }

        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        public async Task SaveChanges()
        {
            // Filter accounts need to be updated
            var studentUserList = _userService.GetStudentUsers();
            List<StudentDisplay> accountsNeedToBeUpdated = StudentAccountList.Where(a => a.ActivateStatus != a.PrimaryStatus).ToList();
            var accept = await  Application.Current.MainPage.DisplayAlert("Question", "Do you want to change Active Status for these students?", "Yes", "No");
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
                        User user = await _userService.AddUser(newUser);
                        if(user == null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Error occurred!", "OK");
                            return;
                        }
                    }
                    else
                    {
                        // Delete user from database
                        User deleteUser = studentUserList.Result.Find(u => u.Username == account.StudentSpecificId);
                        await _userService.DeleteUser(deleteUser.Id);
                    }
                }
                await Application.Current.MainPage.DisplayAlert("Success", "Changes have been saved", "OK");
                getStudentAccountCommand.Execute(null);
            }
        }

        public bool CanSaveChanges()
        {
            var account = primaryStudentAccountList.Find(a => a.PrimaryStatus != a.ActivateStatus);

            return (account != null);
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

        public void DisplayStudentAccountInformation(Student student)
        {
            AvatarUri = "profile_avatar.jpg";
            FullName = student.FullName;
            StudentSpecificId = student.StudentSpecificId;
            Email = student.StudentSpecificId.Substring(2) + "@gm.uit.edu.vn";
            Gender = student.Gender.ToString();
            DateOfBirth = student.DateOfBirth.ToString("dd/MM/yyyy");
            Branch = student.Branch.BranchName;
            Department = student.Branch.Department.DepartmentName;
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {             
                switch (newValue)
                {
                    case "Name":
                        StudentAccountList = primaryStudentAccountList.OrderBy(a => a.FullName).ToObservableCollection();
                        break;
                    case "Student ID":
                        StudentAccountList = primaryStudentAccountList.OrderBy(a => a.StudentSpecificId).ToObservableCollection();
                        break;
                }

                SearchFilter = "";
                ResetItemBackgrounds();
                ResetAccountInformation();
            }
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            
            switch (SelectedFilterOption)
            {
                case "Name":
                    SearchStudentsByName(newValue);
                    break;
                case "Student ID":
                    StudentAccountList = primaryStudentAccountList.Where(a => a.StudentSpecificId.Contains(newValue)).OrderBy(a => a.StudentSpecificId).ToObservableCollection();
                    break;
            }

            ResetItemBackgrounds();
            ResetAccountInformation();

        }
        #endregion

        #region Helpers
        private async void ReloadStudentAccountList(List<Student> accountList)
        {
            primaryStudentAccountList.Clear();

            if (accountList.Count > 0)
            {
                for(int i = 0; i < accountList.Count; i++)
                {
                    primaryStudentAccountList.Add(new StudentDisplay
                    {
                        FullName = accountList[i].FullName,
                        StudentSpecificId = accountList[i].StudentSpecificId,
                        Email = accountList[i].StudentSpecificId.Substring(2) + "@gm.uit.edu.vn",
                        Gender = accountList[i].Gender,
                        DateOfBirth = accountList[i].DateOfBirth,
                        Branch = accountList[i].Branch,
                        Department = accountList[i].Branch.Department.DepartmentName,
                        Avatar = "profile_avatar.jpg",
                        ActivateStatus = await IsStudentHasAccount(accountList[i]),
                        PrimaryStatus = await IsStudentHasAccount(accountList[i]),
                        CheckBoxColor = Color.FromArgb("#3189CC"),
                        BackgroundColor = Color.FromArgb("#EBF6FF"),
                        StudentRequester = this,
                    });
                }

                StudentAccountList = primaryStudentAccountList.OrderBy(a => a.FullName).ToObservableCollection();
                for (int i = 0; i < primaryStudentAccountList.Count; i++)
                {
                    StudentAccountList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
                }
            }
        }

        private void ResetAccountInformation()
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
            List<User> userList = await _userService.GetStudentUsers();
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

        string RemoveAccents(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        void SearchStudentsByName(string newValue)
        {
            string normalizedValue = RemoveAccents(newValue);

            string pattern = string.Join(".*", normalizedValue.Select(c => Regex.Escape(c.ToString())));
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            StudentAccountList = primaryStudentAccountList
                .Where(a => regex.IsMatch(RemoveAccents(a.FullName)))
                .OrderBy(a => a.FullName)
                .ToObservableCollection();
        }

        public void NotifyCanSaveChanges()
        {
            SaveChangesCommand.NotifyCanExecuteChanged();
        }

        #endregion
    }
    #endregion
}
