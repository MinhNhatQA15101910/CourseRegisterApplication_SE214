using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;
using CourseRegisterApplication.MAUI.ViewModels.Displays;
using CourseRegisterApplication.MAUI.Views;
using System;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class CourseRegistrationViewModel : ObservableObject, ISubjectRequester, ISubjectRequester2
	{
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubjectService _subjectService;
        private readonly ISubjectTypeService _subjectTypeService;
        private readonly ISemesterService _semesterService;
        private readonly IStudentService _studentService;
        private readonly IPriorityTypeService _priorityTypeService;
        private readonly IStudentPriorityTypeService _studentPriorityTypeService;
        private readonly IAvailableCourseService _availableCourseService;
        private readonly ICourseRegistrationFormService _courseRegistrationFormService;
        private readonly ICourseRegistrationDetailService _courseRegistrationDetailService;
        #endregion

        #region Constructor
        public CourseRegistrationViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _subjectService = _serviceProvider.GetService<ISubjectService>();
            _subjectTypeService = _serviceProvider.GetService<ISubjectTypeService>();
            _semesterService = _serviceProvider.GetService<ISemesterService>();
            _studentService = _serviceProvider.GetService<IStudentService>();
            _priorityTypeService = _serviceProvider.GetService<IPriorityTypeService>();
            _studentPriorityTypeService = _serviceProvider.GetService<IStudentPriorityTypeService>();
            _availableCourseService = _serviceProvider.GetService<IAvailableCourseService>();
            _courseRegistrationFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();
            _courseRegistrationDetailService = _serviceProvider.GetService<ICourseRegistrationDetailService>();

        }
        #endregion

        #region Properties
        private List<Semester> semesterList = new();
        private string selectedSubjectId;
        private string selectedSubjectId2;
        private List<Subject> subjectList = new();
        private List<Subject> availableSubjectList = new();
        private List<Subject> currentSubjectList = new();
        private List<SubjectType> subjectTypeList = new();
        private List<Student> studentList = new();
        private List<AvailableCourse> availableCourseList = new();
        private CourseRegistrationForm courseRegistrationForm = new CourseRegistrationForm();
        private Semester thisSemester = new Semester();
        private Student thisStudent = new Student();



        private readonly List<SubjectDisplay> primarySubjectDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay> subjectDisplayList = new();

        [ObservableProperty]
        private bool isVisbleUnableCourseRegistration;

        [ObservableProperty]
        private bool isVisbleCourseRegistration;

        [ObservableProperty]
        private string unableNotificationText;

        [ObservableProperty]
        private string currentSemester;

        [ObservableProperty]
        private string currentYear; 

        [ObservableProperty]
        private int currentTotalCredits;

        [ObservableProperty]
        private string buttonText;
        
        [ObservableProperty]
        private ObservableCollection<string> filterOptions = new();

        [ObservableProperty]
        private string selectedFilterOption = "";

        [ObservableProperty]
        private string searchFilter = "";

        private readonly List<SubjectDisplay2> primarySubjectDisplayList2 = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay2> subjectDisplayList2 = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay2> currentSubjectDisplayList = new();

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
        public async Task GetSetup()
        {
            await GetAllSubjects();
            await GetAllStudents();
            await GetSubjectType();
            await GetAllSemesters();

            thisSemester = semesterList.OrderByDescending(x => x.Id).FirstOrDefault();
            if(thisSemester != null)
            {
                if (thisSemester.StartRegistrationDate > DateTime.Now)
                {
                    IsVisbleUnableCourseRegistration = true;
                    IsVisbleCourseRegistration = false;
                    UnableNotificationText = "The course registration schedule starts on " + thisSemester.StartRegistrationDate.ToString("dd/MM/yyyy") + ", Please wait to register.";
                }
                else if (thisSemester.EndRegistrationDate < DateTime.Now)
                {
                    IsVisbleUnableCourseRegistration = true;
                    IsVisbleCourseRegistration = false;
                    UnableNotificationText = "The course registration has ended on " + thisSemester.EndRegistrationDate.ToString("dd/MM/yyyy") + ", Please wait to register for the next semester.";
                }
                else if (thisSemester.IsEnded)
                {
                    IsVisbleUnableCourseRegistration = true;
                    IsVisbleCourseRegistration = false;
                    UnableNotificationText = "The course registration has ended, Please wait to register for the next semester.";
                }
                else
                {
                    await GetAvailableCourseBySemesterId(thisSemester.Id);
                    thisStudent = studentList.Find(item => item.StudentSpecificId == GlobalConfig.CurrentUser.Username);
                    await GetCourseRegistrationFormByStudentIdAndSemesterId(thisStudent.Id, thisSemester.Id);

                    if (courseRegistrationForm != null && courseRegistrationForm.State == CourseRegistrationFormState.Confirmed)
                    {
                        IsVisbleUnableCourseRegistration = true;
                        IsVisbleCourseRegistration = false;
                        UnableNotificationText = "Your course registration form has been confirmed.";
                    }
                    else if (courseRegistrationForm != null && courseRegistrationForm.State == CourseRegistrationFormState.Expired)
                    {
                        IsVisbleUnableCourseRegistration = true;
                        IsVisbleCourseRegistration = false;
                        UnableNotificationText = "Your course registration form has been expired.";
                    }
                    else
                    {
                        IsVisbleUnableCourseRegistration = false;
                        IsVisbleCourseRegistration = true;

                        CheckedButtontext();

                        if (thisSemester.SemesterName.ToString() == "FirstSemester")
                        {
                            CurrentSemester = "Học kỳ I";
                        }
                        else if (thisSemester.SemesterName.ToString() == "SecondSemester")
                        {
                            CurrentSemester = "Học kỳ II";
                        }
                        else if (thisSemester.SemesterName.ToString() == "SummerSemester")
                        {
                            CurrentSemester = "Học kỳ hè";
                        }
                        CurrentYear = thisSemester.Year.ToString();

                        FilterOptions.Clear();
                        FilterOptions.Add("Subject ID");
                        FilterOptions.Add("Subject Name");
                        SelectedFilterOption = FilterOptions[0];

                        GetAvailableSubjectList();
                        ReloadSubjectDisplays2(availableSubjectList);

                        await GetCurrentSubjectList(thisStudent.Id, thisSemester.Id);
                        ReloadSubjectDisplays(currentSubjectList);
                        CheckedSubjectDisplayList2();
                    }
                }
            }
        }

        [RelayCommand]
        public void AddToCourseRegistrationForm()
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

                        CheckedTotalCredits();
                        FilterChange();
                        SearchFilter = "";
                        ReloadItemsBackground();
                        ReloadItemsBackground2();
                        CheckedSubjectDisplayList2();
                    }
                }
                selectedSubjectId2 = null;
            }
        }

        [RelayCommand]
        public void DeleteFromCourseRegistrationForm()
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

                        CheckedTotalCredits();
                        FilterChange();
                        SearchFilter = "";
                        ReloadItemsBackground();
                        ReloadItemsBackground2();
                        CheckedSubjectDisplayList2();
                    }
                }
                selectedSubjectId = null;
            }
        }

        [RelayCommand]
        public async Task SaveChanged()
        {
            if(CurrentTotalCredits < thisSemester.MinimumCredits || CurrentTotalCredits > thisSemester.MaximumCredits)
            {
                string alert = "Your credits must be within the range of " + thisSemester.MinimumCredits + " to " + thisSemester.MaximumCredits;
                await Application.Current.MainPage.DisplayAlert("Warning!", alert, "Cancel");
            }
            else
            {
                bool accept;
                if (courseRegistrationForm != null)
                {
                    accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to update this course regstration form?", "Yes", "No");
                }
                else
                {
                    accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to create this course regstration form?", "Yes", "No");
                }
                if (accept)
                {
                    if (courseRegistrationForm == null)
                    {
                        CourseRegistrationForm cRF = new CourseRegistrationForm();
                        cRF.CreatedDate = DateTime.Now;
                        cRF.StudentId = thisStudent.Id;
                        cRF.SemesterId = thisSemester.Id;
                        cRF.CourseRegistrationFormSpecificId = "CRF" + cRF.Id;
                        cRF.State = CourseRegistrationFormState.Pending;
                        await _courseRegistrationFormService.CreateCourseRegistrationForm(cRF);
                        await GetCourseRegistrationFormByStudentIdAndSemesterId(cRF.StudentId, cRF.SemesterId);
                        CheckedButtontext();
                    }

                    List<Subject> deleteSubjectList = currentSubjectList
                        .Where(c => !primarySubjectDisplayList.Select(c => c.SubjectID).Contains(c.SubjectSpecificId))
                        .Select(c => new Subject { SubjectSpecificId = c.SubjectSpecificId, Id = c.Id })
                        .ToList();
                    List<CourseRegistrationDetail> addSubjectList = primarySubjectDisplayList
                        .Where(c => !currentSubjectList.Select(c => c.SubjectSpecificId).Contains(c.SubjectID))
                        .Select(c =>
                        {
                            var newItem = new Subject { SubjectSpecificId = c.SubjectID };
                            var matchingItem = subjectList.Find(item2 => item2.SubjectSpecificId == newItem.SubjectSpecificId);
                            if (matchingItem != null)
                            {
                                return new CourseRegistrationDetail
                                {
                                    CourseRegistrationFormId = courseRegistrationForm.Id,
                                    SubjectId = matchingItem.Id,
                                };
                            }
                            return null;
                        })
                        .Where(curriculum => curriculum != null)
                        .ToList();
                    foreach (var item in deleteSubjectList)
                    {
                        await _courseRegistrationDetailService.DeleteCourseRegistrationDetail(courseRegistrationForm.Id, item.Id);
                    }
                    foreach (var item in addSubjectList)
                    {
                        await _courseRegistrationDetailService.CreateCourseRegistrationDetail(item);
                    }
                    List<CourseRegistrationDetail> cRD = await _courseRegistrationDetailService.GetAllCRD();

                    List<CourseRegistrationDetail> currentCRD = cRD
                        .Where(item => item.CourseRegistrationFormId == courseRegistrationForm.Id)
                        .ToList();

                    List<Subject> currentSubject = currentCRD
                        .Join(subjectList, item => item.SubjectId, item2 => item2.Id, (item, item2) => item2)
                        .ToList();

                    double TotalPrice = currentSubject
                        .Join(subjectTypeList, item => item.SubjectTypeId, item2 => item2.Id, (item, item2) =>
                            item.NumberOfCredits * item2.LessonsCharge)
                        .Sum();

                    courseRegistrationForm.TotalCharge= TotalPrice;

                    List<StudentPriorityType> studentPriorityTypeList = await _studentPriorityTypeService.GetStudentPriorityTypesByStudentId(thisStudent.Id);
                    List<PriorityType> priorityTypeList = await _priorityTypeService.GetAllPriorityType();
                    PriorityType maxPriorityType = priorityTypeList
                    .Where(item => studentPriorityTypeList.Exists(c => c.PriorityTypeId == item.Id))
                    .OrderByDescending(item => item.TuitionDiscountRate)
                    .FirstOrDefault();
                    courseRegistrationForm.TotalChargeWithDiscount = TotalPrice * (1 - maxPriorityType.TuitionDiscountRate);

                    courseRegistrationForm.CreatedDate = DateTime.Now;
                    await _courseRegistrationFormService.UpdateCourseRegistrationForm(courseRegistrationForm.Id, courseRegistrationForm);

                    await GetCurrentSubjectList(thisStudent.Id, thisSemester.Id);
                    ReloadSubjectDisplays(currentSubjectList);
                    CheckedSubjectDisplayList2();
                }
            }
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                SearchFilter = "";
                FilterChange();
                CheckedSubjectDisplayList2();
            }
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            SubjectDisplayList2 = primarySubjectDisplayList2.ToObservableCollection();
            FilterChange();
            SubjectDisplayList2 = SubjectDisplayList2.Where(a => a.SubjectName2.ToLower().Contains(newValue.ToLower()) || a.SubjectID2.ToLower().Contains(newValue.ToLower())).ToObservableCollection();
            CheckedSubjectDisplayList2();
        }
        #endregion

        #region Helper
        public void GetAvailableSubjectList()
        {
            availableSubjectList.Clear();
            availableSubjectList.AddRange(
               from item in availableCourseList
               join item2 in subjectList on item.SubjectId equals item2.Id
               select item2);
        }

        public async Task GetCurrentSubjectList(int studentId, int semesterId)
        {
            currentSubjectList.Clear();
            await GetCourseRegistrationFormByStudentIdAndSemesterId(studentId, semesterId);
            if(courseRegistrationForm!=null)
            {
                List<CourseRegistrationDetail> courseRegistrationDetailList = await _courseRegistrationDetailService.GetAllCRD();
                var curentCRDList = courseRegistrationDetailList.Where(item => item.CourseRegistrationFormId == courseRegistrationForm.Id).ToList();
                currentSubjectList.AddRange(
                   from item in curentCRDList
                   join item2 in subjectList on item.SubjectId equals item2.Id
                   select item2);
            }
        }

        public async Task GetAllSubjects()
        {
            subjectList = await _subjectService.GetAllSubjects();
        }

        public async Task GetAllStudents()
        {
            studentList = await _studentService.GetStudents();
        }

        public async Task GetSubjectType()
        {
            subjectTypeList = await _subjectTypeService.GetAllSubjectType();
        }
        public async Task GetAllSemesters()
        {
            semesterList = await _semesterService.GetAllSemester();
        }
        public async Task GetAvailableCourseBySemesterId(int semesterId)
        {
            availableCourseList = await _availableCourseService.GetAvailableCourseBySemesterId(semesterId);
        }
        public async Task GetCourseRegistrationFormByStudentIdAndSemesterId(int studentId, int semesterId)
        {
            courseRegistrationForm = await _courseRegistrationFormService.GetCourseRegistrationFormByStudentIdAndSemesterId(studentId, semesterId);
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
                CheckedTotalCredits();
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

        private void CheckedButtontext()
        {
            if (courseRegistrationForm == null)
            {
                ButtonText = "Register";
            }
            else
            {
                ButtonText = "Save changed";
            }
        }

        private void CheckedTotalCredits()
        {
            CurrentTotalCredits = 0;
            foreach (var item in SubjectDisplayList)
            {
                CurrentTotalCredits += item.NumberOfCredits;
            }
        }
        private void CheckedSubjectDisplayList2()
        {
            if (SubjectDisplayList2 != null && SubjectDisplayList != null)
            {
                SubjectDisplayList2 = SubjectDisplayList2.Where(s2 => !SubjectDisplayList.Any(s1 => s1.SubjectID == s2.SubjectID2)).ToObservableCollection();
                ReloadItemsBackground2();
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
