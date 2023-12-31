using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.ViewModels.Displays;
namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{

    public partial class UpdateCurriculumViewModel : ObservableObject, ISubjectRequester, ISubjectRequester2
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubjectService _subjectService;
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        private readonly ICurriculumService _curriculumService;
        private readonly ISubjectTypeService _subjectTypeService;
        #endregion

        #region Properties
        private List<Department> departmentList = new();
        private List<Branch> branchList = new();
        private int selectedBranchID = 0;
        private int selectedSemester = 0;
        private string selectedSubjectId;
        private string selectedSubjectId2;
        private List<Subject> subjectList = new();
        private List<Subject> unselectedSubjectList = new();
        private List<Curriculum> curriculumList = new();
        private List<Subject> currentSubjectList = new();
        private List<Curriculum> currentCurriculumList = new();
        private List<SubjectType> subjectTypeList = new();

        private readonly List<SubjectDisplay> primarySubjectDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay> subjectDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<string> filterOptions = new();

        [ObservableProperty]
        private string selectedFilterOption = "";

        [ObservableProperty]
        private string searchFilter = "";

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

        private readonly List<SubjectDisplay2> primarySubjectDisplayList2 = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay2> subjectDisplayList2 = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay2> currentSubjectDisplayList = new();

        #endregion

        #region Constructor
        public UpdateCurriculumViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _subjectService = _serviceProvider.GetRequiredService<ISubjectService>();
            _departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            _branchService = _serviceProvider.GetRequiredService<IBranchService>();
            _curriculumService = _serviceProvider.GetRequiredService<ICurriculumService>();
            _subjectTypeService = _serviceProvider.GetRequiredService<ISubjectTypeService>();
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
            await GetSubjectType();

            if (departmentList.Count > 0)
            {
                DepartmentFilterOptions.Clear();
                foreach (var item in departmentList)
                {
                    DepartmentFilterOptions.Add(item.DepartmentSpecificId);
                }
                SelectedDepartmentFilterOption = DepartmentFilterOptions[0];
            }

            SemesterFilterOptions.Clear();
            for (int i = 0; i < 8; i++)
            {
                SemesterFilterOptions.Add("Học kỳ " + (i + 1));
            }

            FilterOptions.Add("Subject ID");
            FilterOptions.Add("Subject Name");
            SelectedFilterOption = FilterOptions[0];

            SelectedSemesterFilterOption = SemesterFilterOptions[0];
            ReloadSubjectDisplays(currentSubjectList);

            await GetUnselectedSubject(selectedBranchID);
            ReloadSubjectDisplays2(unselectedSubjectList);
        }

        [RelayCommand]
        public async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand]
        public void AddToCurriculum()
        {
            if (selectedSubjectId2 != null)
            {
                var selectedSubject = subjectList.Find(c => c.SubjectSpecificId == selectedSubjectId2);
                if (selectedSubject != null)
                {
                    var subjectType = subjectTypeList.Find(c => c.Id == selectedSubject.SubjectTypeId);
                    if (subjectType != null)
                    {
                        SubjectDisplay subjectDisplay = new SubjectDisplay
                        {
                            SubjectRequester = this,
                            SubjectID = selectedSubject.SubjectSpecificId,
                            SubjectName = selectedSubject.Name,
                            NumberOfCredits = selectedSubject.NumberOfCredits,
                            SubjectType = subjectType.Name,
                        };
                        primarySubjectDisplayList.Add(subjectDisplay);
                        primarySubjectDisplayList2.RemoveAll(c => c.SubjectID2 == selectedSubjectId2);

                        SubjectDisplayList = primarySubjectDisplayList.OrderBy(d => d.SubjectID).ToObservableCollection();
                        SubjectDisplayList2 = primarySubjectDisplayList2.OrderBy(d => d.SubjectID2).ToObservableCollection();

                        FilterChange();
                        SearchFilter = "";
                        ReloadItemsBackground();
                        ReloadItemsBackground2();
                    }
                }
                selectedSubjectId2 = null;
            }
        }

        [RelayCommand]
        public void DeleteFromCurriculum()
        {
            if (selectedSubjectId != null)
            {
                var selectedSubject = subjectList.Find(c => c.SubjectSpecificId == selectedSubjectId);
                if (selectedSubject != null)
                {
                    var subjectType = subjectTypeList.Find(c => c.Id == selectedSubject.SubjectTypeId);
                    if (subjectType != null)
                    {
                        SubjectDisplay2 subjectDisplay2 = new SubjectDisplay2
                        {
                            SubjectRequester2 = this,
                            SubjectID2 = selectedSubject.SubjectSpecificId,
                            SubjectName2 = selectedSubject.Name,
                            NumberOfCredits2 = selectedSubject.NumberOfCredits,
                            SubjectType2 = subjectType.Name,
                        };
                        primarySubjectDisplayList2.Add(subjectDisplay2);
                        primarySubjectDisplayList.RemoveAll(c => c.SubjectID == selectedSubjectId);

                        SubjectDisplayList = primarySubjectDisplayList.OrderBy(d => d.SubjectID).ToObservableCollection();
                        SubjectDisplayList2 = primarySubjectDisplayList2.OrderBy(d => d.SubjectID2).ToObservableCollection();

                        FilterChange();
                        SearchFilter = "";
                        ReloadItemsBackground();
                        ReloadItemsBackground2();
                    }
                }
                selectedSubjectId = null;
            }
        }

        [RelayCommand]
        public async Task SaveChanged()
        {
            List<Subject> deleteSubjectList = currentSubjectList
                .Where(c => !primarySubjectDisplayList.Select(c => c.SubjectID).Contains(c.SubjectSpecificId))
                .Select(c => new Subject { SubjectSpecificId = c.SubjectSpecificId, Id=c.Id })
                .ToList();
            List<Curriculum> addSubjectList = primarySubjectDisplayList
                .Where(c => !currentSubjectList.Select(c => c.SubjectSpecificId).Contains(c.SubjectID))
                .Select(c =>
                {
                    var newItem = new Subject { SubjectSpecificId = c.SubjectID };
                    var matchingItem = subjectList.Find(item2 => item2.SubjectSpecificId == newItem.SubjectSpecificId);
                    if (matchingItem != null)
                    {
                        return new Curriculum
                        {
                            BranchId = selectedBranchID,
                            SubjectId = matchingItem.Id,
                            Semester = selectedSemester
                        };
                    }
                    return null;
                })
                .Where(curriculum => curriculum != null)
                .ToList();

            bool accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to update this curriculum", "Yes", "No");
            if (accept)
            {
                foreach (var item in deleteSubjectList)
                {
                    await _curriculumService.DeleteCurriculum(selectedBranchID, item.Id);
                }
                foreach (var item in addSubjectList)
                {
                    await _curriculumService.AddCurriculum(item);
                }
                SearchFilter = "";
                await GetAllCurriculums();
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);

                await GetUnselectedSubject(selectedBranchID);
                ReloadSubjectDisplays2(unselectedSubjectList);
            }

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
                    if (item.DepartmentSpecificId == newValue)
                    {
                        foreach (var item2 in branchList)
                        {
                            if (item.Id == item2.DepartmentId)
                            {
                                BranchFilterOptions.Add(item2.BranchSpecificId);
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
                    if (item.BranchSpecificId == newValue)
                    {
                        selectedBranchID = item.Id;
                        break;
                    }
                }
                SearchFilter = "";
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);
                GetUnselectedSubject(selectedBranchID);
                ReloadSubjectDisplays2(unselectedSubjectList);
            }
        }

        partial void OnSelectedSemesterFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                SearchFilter = "";
                selectedSemester = SemesterFilterOptions.IndexOf(SelectedSemesterFilterOption)+1;
                GetSubjectList(selectedBranchID, selectedSemester);
                ReloadSubjectDisplays(currentSubjectList);
                GetUnselectedSubject(selectedBranchID);
                ReloadSubjectDisplays2(unselectedSubjectList);
            }
        }

        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                SearchFilter = "";
                FilterChange();
            }
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            SubjectDisplayList2 = primarySubjectDisplayList2.ToObservableCollection();
            FilterChange();
            SubjectDisplayList2 = SubjectDisplayList2.Where(a => a.SubjectName2.ToLower().Contains(newValue.ToLower()) || a.SubjectID2.ToLower().Contains(newValue.ToLower())).ToObservableCollection();
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

        public async Task GetUnselectedSubject(int selectedBranchID)
        {
            curriculumList = await _curriculumService.GetAllCurriculums();
            List<Curriculum> curriculumByBranch = await _curriculumService.GetCurriculumsByBranchId(selectedBranchID);
            unselectedSubjectList = subjectList
                .Where(subject => !curriculumByBranch.Exists(item => item.SubjectId == subject.Id))
                .ToList();
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
        public async Task GetSubjectType()
        {
            subjectTypeList = await _subjectTypeService.GetAllSubjectType();
        }
        private void ReloadSubjectDisplays(List<Subject> subjectList)
        {
            primarySubjectDisplayList.Clear();
            SubjectDisplayList = null;

            if (subjectList.Count > 0)
            {
                primarySubjectDisplayList.AddRange(
                from item in subjectList
                join item2 in subjectTypeList on item.SubjectTypeId equals item2.Id
                select new SubjectDisplay
                {
                    SubjectRequester = this,
                    SubjectID = item.SubjectSpecificId,
                    SubjectName = item.Name,
                    NumberOfCredits = item.NumberOfCredits,
                    SubjectType = item2.Name,
                });

                SubjectDisplayList = primarySubjectDisplayList.OrderBy(d => d.SubjectID).ToObservableCollection();
                FilterChange();
                ReloadItemsBackground();
            }
        }

        private void ReloadSubjectDisplays2(List<Subject> subjectList)
        {
            primarySubjectDisplayList2.Clear();
            SubjectDisplayList2 = null;

            if (subjectList.Count > 0)
            {
                primarySubjectDisplayList2.AddRange(
                from item in subjectList
                join item2 in subjectTypeList on item.SubjectTypeId equals item2.Id
                select new SubjectDisplay2
                {
                    SubjectRequester2 = this,
                    SubjectID2 = item.SubjectSpecificId,
                    SubjectName2 = item.Name,
                    NumberOfCredits2 = item.NumberOfCredits,
                    SubjectType2 = item2.Name,
                });
                var differenceList = primarySubjectDisplayList2
                    .Where(c => !primarySubjectDisplayList.Exists(d => d.SubjectID == c.SubjectID2))
                    .ToList();
                SubjectDisplayList2 = differenceList.OrderBy(d => d.SubjectID2).ToObservableCollection();
                FilterChange();
                ReloadItemsBackground2();
            }
        }

        public void ReloadItemsBackground()
        {
            if (SubjectDisplayList != null && SubjectDisplayList.Count > 0)
            {
                for (int i = 0; i < SubjectDisplayList.Count; i++)
                {
                    SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
                }
            }
            
        }

        public void ChooseSubject(SubjectDisplay subjectDisplay)
        {
            selectedSubjectId = subjectDisplay.SubjectID;
            selectedSubjectId2 = null;
            ReloadItemsBackground2();
        }

        public void ReloadItemsBackground2()
        {
            for (int i = 0; i < SubjectDisplayList2.Count; i++)
            {
                SubjectDisplayList2[i].BackgroundColor2 = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void ChooseSubject2(SubjectDisplay2 subjectDisplay2)
        {
            if (SubjectDisplayList2.Count > 0)
            {
                selectedSubjectId2 = subjectDisplay2.SubjectID2;
                selectedSubjectId = null;
                ReloadItemsBackground();
            }
        }
        public void FilterChange()
        {
            if (FilterOptions.Count > 0)
            {
                if (SelectedFilterOption == FilterOptions[0])
                {
                    if (SubjectDisplayList2 != null)
                    {
                        SubjectDisplayList2 = SubjectDisplayList2.OrderBy(item => item.SubjectID2).ToObservableCollection();
                    }
                }
                else
                {
                    if (SubjectDisplayList2 != null)
                    {
                        SubjectDisplayList2 = SubjectDisplayList2.OrderBy(item => item.SubjectName2).ToObservableCollection();
                    }

                }
            }
        }
        #endregion
    }
}
