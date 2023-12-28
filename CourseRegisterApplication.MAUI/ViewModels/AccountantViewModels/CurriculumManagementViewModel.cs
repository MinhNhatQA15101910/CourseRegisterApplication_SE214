using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;
namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class SubjectDisplay : ObservableObject
    {
        #region Properties
        public ISubjectRequester SubjectRequester { get; set; }

        [ObservableProperty]
        private string subjectID;

        [ObservableProperty]
        private string subjectName;

        [ObservableProperty]
        private int numberOfCredits;

        [ObservableProperty]
        private string semester;

        [ObservableProperty]
        private Color backgroundColor;
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseSubject()
        {
            SubjectRequester.ReloadItemsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");
            SubjectRequester.ChooseSubject(this);

        }
        #endregion
    }

    #region IRequesters
    public interface ISubjectRequester
    {
        void ReloadItemsBackground();
        void ChooseSubject(SubjectDisplay subjectDisplay);
    }
    #endregion
    public partial class CurriculumManagementViewModel : ObservableObject, ISubjectRequester
    {

        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubjectService _subjectService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly ICurriculumService _curriculumService;
        #endregion

        #region Properties
        private List<Department> departmentList = new();
        private List<Branch> branchList = new();
        private int selectedDepartmentID;
        private int selectedBranchID;
        private int selectedSemester;
        private string selectedSubjectId;
        private List<Subject> subjectList = new();
        private List<Curriculum> curriculumList = new();
        private List<Subject> currentSubjectList = new();
        private List<Curriculum> currentCurriculumList = new();

        private readonly List<SubjectDisplay> primarySubjectDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay> subjectDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<string> departmentFilterOptions = new();

        [ObservableProperty]
        private string selectedDepartmentFilterOption = "";

        [ObservableProperty]
        private ObservableCollection<string> branchFilterOptions = new();

        [ObservableProperty]
        private string selectedBranchFilterOption = "";

        [ObservableProperty]
        private ObservableCollection<string> semesterFilterOptions = new();

        [ObservableProperty]
        private string selectedSemesterFilterOption = "";
        #endregion

        #region Constructor
        public CurriculumManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _subjectService = _serviceProvider.GetRequiredService<ISubjectService>();
            _departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            _branchService = _serviceProvider.GetRequiredService<IBranchService>();
            _curriculumService = _serviceProvider.GetRequiredService<ICurriculumService>();
        }
        #endregion

        #region Command
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
        public async Task GetFilterOption()
        {
            await GetAllDepartments();
            await GetAllBranches();
            await GetAllSubjects();
            await GetAllCurriculums();

            if (departmentList.Count > 0)
            {
                DepartmentFilterOptions.Clear();
                foreach (var item in departmentList)
                {
                    DepartmentFilterOptions.Add(item.DepartmentName);
                }
                SelectedDepartmentFilterOption = DepartmentFilterOptions[0];
            }
            SemesterFilterOptions.Clear();
            SemesterFilterOptions.Add("All");
            for (int i = 0; i < 8; i++)
            {
                SemesterFilterOptions.Add("Học kỳ " + (i + 1));
            }
            SelectedSemesterFilterOption = SemesterFilterOptions[0];
            ReloadSubjectDisplays(currentSubjectList);
        }

        [RelayCommand]
        public async Task DeleteCurriculum()
        {
            bool accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to delete this curriculum", "Yes", "No");
            if (accept)
            {
                foreach (var item in currentCurriculumList)
                {
                    await _curriculumService.DeleteCurriculum(item.BranchId, item.SubjectId);
                }
                await GetAllCurriculums();
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);
            }
        }

        [RelayCommand]
        public async Task UpdateCurriculum()
        {
            await Shell.Current.GoToAsync(nameof(UpdateCurriculumPage), true);
        }
        #endregion

        #region Property Changed
        partial void OnSelectedDepartmentFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                BranchFilterOptions.Clear();
                foreach (var item in departmentList)
                {
                    if (item.DepartmentName == newValue)
                    {
                        selectedDepartmentID = item.Id;
                        foreach (var item2 in branchList)
                        {
                            if (item.Id == item2.DepartmentId)
                            {
                                BranchFilterOptions.Add(item2.BranchName);
                            }
                        }
                        SelectedBranchFilterOption = BranchFilterOptions[0];
                        break;
                    }
                }
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);
            }
        }

        partial void OnSelectedBranchFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                foreach (var item in branchList)
                {
                    if (item.BranchName == newValue)
                    {
                        selectedBranchID = item.Id;
                        break;
                    }
                }
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);
            }
        }

        partial void OnSelectedSemesterFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                selectedSemester = SemesterFilterOptions.IndexOf(SelectedSemesterFilterOption);
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);
            }
        }
        #endregion

        #region Helpers
        public void GetSubjectList(int branchId, int semester)
        {
            currentCurriculumList = curriculumList
                .Where(c => c.BranchId == branchId)
                .ToList();
            if (semester != 0)
            {
                currentCurriculumList = currentCurriculumList
                    .Where(c => c.Semester == semester)
                    .ToList();
            }
            currentSubjectList.Clear();
            currentSubjectList.AddRange(
                from item in currentCurriculumList
                join item2 in subjectList on item.SubjectId equals item2.Id
                select item2);
        }

        public async Task GetAllDepartments()
        {
            departmentList = await _departmentService.GetAllDepartments();
        }
        public async Task GetAllBranches()
        {
            branchList = await _branchService.GetAllBranches();
        }
        public async Task GetAllSubjects()
        {
            subjectList = await _subjectService.GetAllSubjects();
        }
        public async Task GetAllCurriculums()
        {
            curriculumList = await _curriculumService.GetAllCurriculums();
        }
        private void ReloadSubjectDisplays(List<Subject> subjectList)
        {
            primarySubjectDisplayList.Clear();
            SubjectDisplayList = null;

            if (subjectList.Count > 0)
            {
                primarySubjectDisplayList.AddRange(
                from item in subjectList
                join item2 in currentCurriculumList on item.Id equals item2.SubjectId
                select new SubjectDisplay
                {
                    SubjectRequester = this,
                    SubjectID = item.SubjectSpecificId,
                    SubjectName = item.Name,
                    NumberOfCredits = item.NumberOfCredits,
                    Semester = "Học kỳ " + item2.Semester
                });

                SubjectDisplayList = primarySubjectDisplayList.OrderBy(d => d.Semester).ThenBy(d => d.SubjectID).ToObservableCollection();
                ReloadItemsBackground();
            }
        }

        public void ReloadItemsBackground()
        {
            for (int i = 0; i < SubjectDisplayList.Count; i++)
            {
                SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void ChooseSubject(SubjectDisplay subjectDisplay)
        {
            selectedSubjectId = subjectDisplay.SubjectID;
        }
        #endregion
    }
}
