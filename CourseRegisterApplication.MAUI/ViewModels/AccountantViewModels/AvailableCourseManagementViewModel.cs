using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

#region Displays
public partial class SubjectForAvailableCoursesDiplay : ObservableObject
{
    #region Properties
    public ISubjectForAvailableCoursesRequester SubjectForAvailableCoursesRequester { get; set; }

    public string Type { get; set; }

    public int Id { get; set; }

    [ObservableProperty] private string subjectSpecificId;

    [ObservableProperty] private string subjectName;

    [ObservableProperty] private int numberOfCredits;

    [ObservableProperty] private int totalLessons;

    [ObservableProperty] private Color backgroundColor;
    #endregion

    #region Commands
    [RelayCommand]
    private void SaveSubject()
    {
        if (Type == "Selected")
        {
            SubjectForAvailableCoursesRequester.ReloadSelectedSubjectsBackground();
            SubjectForAvailableCoursesRequester.SaveSelectedSubject(this);
        }
        else if (Type == "Not Selected")
        {
            SubjectForAvailableCoursesRequester.ReloadSubjectsBackground();
            SubjectForAvailableCoursesRequester.SaveSubject(this);
        }
        BackgroundColor = Color.FromArgb("#B9D8F2");
    }
    #endregion
}
#endregion

#region Requesters
public interface ISubjectForAvailableCoursesRequester
{
    void SaveSubject(SubjectForAvailableCoursesDiplay subjectForAvailableCoursesDiplay);
    void SaveSelectedSubject(SubjectForAvailableCoursesDiplay subjectForAvailableCoursesDiplay);
    void ReloadSubjectsBackground();
    void ReloadSelectedSubjectsBackground();
}
#endregion

#region Main ViewModel
public partial class AvailableCourseManagementViewModel : ObservableObject, ISubjectForAvailableCoursesRequester
{
    #region Services
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region Properties
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(EndSemesterCommand))] 
    private bool isEnd;

    [ObservableProperty] private string commandName;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SelectSubjectCommand))] 
    private SubjectForAvailableCoursesDiplay savedSubject = null;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UnselectSubjectCommand))]
    private SubjectForAvailableCoursesDiplay savedSelectedSubject = null;

    [ObservableProperty] private ObservableCollection<string> subjectFilterOptions = new() { "ID", "Name" };

    [ObservableProperty] private string selectedSubjectFilterOption = "ID";

    private List<Subject> originalPrimarySubjectList = new();

    private List<SubjectForAvailableCoursesDiplay> primarySubjectDisplayList = new();

    [ObservableProperty] private ObservableCollection<SubjectForAvailableCoursesDiplay> subjectDisplayList = new();

    private List<SubjectForAvailableCoursesDiplay> primarySelectedSubjectDisplayList = new();

    [ObservableProperty] private ObservableCollection<SubjectForAvailableCoursesDiplay> selectedSubjectDisplayList = new();

    [ObservableProperty] private string semesterName;

    [ObservableProperty] private int year;

    [ObservableProperty] private string minCredit;

    [ObservableProperty] private string maxCredit;

    [ObservableProperty] private DateTime startDate;

    [ObservableProperty] private DateTime endDate;

    [ObservableProperty] private string searchFilter;
    #endregion

    #region Constructor
    public AvailableCourseManagementViewModel(IServiceProvider serviceProvider)
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
    public async Task GetSubjects()
    {
        // Get current semester
        ISemesterService semesterService = _serviceProvider.GetService<ISemesterService>();
        Semester semester = await semesterService.GetCurrentSemesterAsync();

        IsEnd = semester.IsEnded;

        // Semester is ended case
        if (semester.IsEnded)
        {
            CommandName = "Add new semester";

            ISubjectService subjectService = _serviceProvider.GetService<ISubjectService>();
            switch (semester.SemesterName)
            {
                case Shared.SemesterName.FirstSemester:
                    SemesterName = "Second Semester";
                    Year = semester.Year;

                    originalPrimarySubjectList = (await subjectService.GetSubjectsForSecondSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SecondSemester:
                    SemesterName = "Summer Semester";
                    Year = semester.Year;

                    originalPrimarySubjectList = (await subjectService.GetSubjectsForSummerSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SummerSemester:
                    SemesterName = "First Semester";
                    Year = semester.Year + 1;

                    originalPrimarySubjectList = (await subjectService.GetSubjectsForFirstSemesterAsync()).ToList();

                    break;
            }

            MinCredit = "0";
            MaxCredit = "0";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            primarySubjectDisplayList.Clear();
            foreach (Subject subject in originalPrimarySubjectList)
            {
                primarySubjectDisplayList.Add(new()
                {
                    Id = subject.Id,
                    SubjectForAvailableCoursesRequester = this, 
                    Type = "Not Selected",
                    SubjectSpecificId = subject.SubjectSpecificId,
                    SubjectName = subject.Name,
                    NumberOfCredits = subject.NumberOfCredits,
                    TotalLessons = subject.TotalLessons
                });
            }
            SubjectDisplayList = primarySubjectDisplayList
                .OrderBy(s => s.SubjectSpecificId)
                .ToObservableCollection();

            primarySelectedSubjectDisplayList.Clear();
            SelectedSubjectDisplayList = primarySelectedSubjectDisplayList
                .OrderBy(s => s.SubjectSpecificId)
                .ToObservableCollection();
        }
        // Semester is not ended case
        else
        {
            CommandName = "Update semester";

            ISubjectService subjectService = _serviceProvider.GetService<ISubjectService>();
            switch (semester.SemesterName)
            {
                case Shared.SemesterName.FirstSemester:
                    SemesterName = "First Semester";

                    originalPrimarySubjectList = (await subjectService.GetSubjectsForFirstSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SecondSemester:
                    SemesterName = "Second Semester";

                    originalPrimarySubjectList = (await subjectService.GetSubjectsForSecondSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SummerSemester:
                    SemesterName = "Summer Semester";

                    originalPrimarySubjectList = (await subjectService.GetSubjectsForSummerSemesterAsync()).ToList();

                    break;
            }

            Year = semester.Year;
            MinCredit = semester.MinimumCredits.ToString();
            MaxCredit = semester.MaximumCredits.ToString();
            StartDate = semester.StartRegistrationDate;
            EndDate = semester.EndRegistrationDate;

            List<Subject> subjectListInAvailableCourses = (await subjectService.GetSubjectsBySemesterIdAsync(semester.Id)).OrderBy(s => s.SubjectSpecificId).ToList();

            primarySelectedSubjectDisplayList.Clear();
            foreach (Subject subject in subjectListInAvailableCourses)
            {
                primarySelectedSubjectDisplayList.Add(new()
                {
                    Id = subject.Id,
                    SubjectForAvailableCoursesRequester = this,
                    Type = "Selected",
                    SubjectSpecificId = subject.SubjectSpecificId,
                    SubjectName = subject.Name,
                    NumberOfCredits = subject.NumberOfCredits,
                    TotalLessons = subject.TotalLessons
                });
            }
            SelectedSubjectDisplayList = primarySelectedSubjectDisplayList
                .OrderBy(s => s.SubjectSpecificId)
                .ToObservableCollection();

            primarySubjectDisplayList.Clear();
            foreach (Subject subject in originalPrimarySubjectList)
            {
                var subjectsInSelectedListWithSameId = subjectListInAvailableCourses
                    .Where(s => s.Id == subject.Id);

                if (!subjectsInSelectedListWithSameId.Any())
                {
                    primarySubjectDisplayList.Add(new()
                    {
                        Id = subject.Id,
                        SubjectForAvailableCoursesRequester = this,
                        Type = "Not Selected",
                        SubjectSpecificId = subject.SubjectSpecificId,
                        SubjectName = subject.Name,
                        NumberOfCredits = subject.NumberOfCredits,
                        TotalLessons = subject.TotalLessons
                    });
                }
            }
            SubjectDisplayList = primarySubjectDisplayList
                .OrderBy(s => s.SubjectSpecificId)
                .ToObservableCollection();
        }

        ReloadSubjectsBackground();
        ReloadSelectedSubjectsBackground();
    }

    [RelayCommand(CanExecute = nameof(CanSelectSubjectCommandExecuted))]
    public void SelectSubject()
    {
        SavedSubject.Type = "Selected";

        primarySelectedSubjectDisplayList.Add(SavedSubject);
        SelectedSubjectDisplayList = primarySelectedSubjectDisplayList
            .OrderBy(s => s.SubjectSpecificId)
            .ToObservableCollection();

        primarySubjectDisplayList.Remove(SavedSubject);
        SubjectDisplayList.Remove(SavedSubject);

        ReloadSubjectsBackground();
        ReloadSelectedSubjectsBackground();
    }

    [RelayCommand(CanExecute = nameof(CanUnselectSubjectCommandExecuted))]
    public void UnselectSubject()
    {
        SavedSelectedSubject.Type = "Not Selected";

        primarySubjectDisplayList.Add(SavedSelectedSubject);
        SubjectDisplayList = primarySubjectDisplayList
            .OrderBy(s => s.SubjectSpecificId)
            .ToObservableCollection();

        primarySelectedSubjectDisplayList.Remove(SavedSelectedSubject);
        SelectedSubjectDisplayList.Remove(SavedSelectedSubject);

        SearchFilter = "";
        SelectedSubjectFilterOption = "ID";

        ReloadSubjectsBackground();
        ReloadSelectedSubjectsBackground();
    }

    private bool CanSelectSubjectCommandExecuted()
    {
        return SavedSubject != null;
    }

    private bool CanUnselectSubjectCommandExecuted()
    {
        return SavedSelectedSubject != null;
    }

    [RelayCommand(CanExecute = nameof(CanEndSemesterCommandExecuted))]
    private async Task EndSemester()
    {
        var accept = await Application.Current.MainPage.DisplayAlert("Question", "Are you sure you want to end this current semester?", "Yes", "No");
        if (accept)
        {
            ISemesterService semesterService = _serviceProvider.GetService<ISemesterService>();
            Semester currentSemester = await semesterService.GetCurrentSemesterAsync();
            currentSemester.IsEnded = true;
            currentSemester.EndRegistrationDate = DateTime.Now;

            bool success = await semesterService.UpdateSemesterAsync(currentSemester.Id, currentSemester);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "This semester has been ended!", "OK");

                IsEnd = true;

                CommandName = "Add new semester";

                ISubjectService subjectService = _serviceProvider.GetService<ISubjectService>();
                switch (currentSemester.SemesterName)
                {
                    case Shared.SemesterName.FirstSemester:
                        SemesterName = "Second Semester";
                        Year = currentSemester.Year;

                        originalPrimarySubjectList = (await subjectService.GetSubjectsForSecondSemesterAsync()).ToList();

                        break;
                    case Shared.SemesterName.SecondSemester:
                        SemesterName = "Summer Semester";
                        Year = currentSemester.Year;

                        originalPrimarySubjectList = (await subjectService.GetSubjectsForSummerSemesterAsync()).ToList();

                        break;
                    case Shared.SemesterName.SummerSemester:
                        SemesterName = "First Semester";
                        Year = currentSemester.Year + 1;

                        originalPrimarySubjectList = (await subjectService.GetSubjectsForFirstSemesterAsync()).ToList();

                        break;
                }

                MinCredit = "0";
                MaxCredit = "0";
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;

                primarySubjectDisplayList.Clear();
                foreach (Subject subject in originalPrimarySubjectList)
                {
                    primarySubjectDisplayList.Add(new()
                    {
                        Id = subject.Id,
                        SubjectForAvailableCoursesRequester = this,
                        Type = "Not Selected",
                        SubjectSpecificId = subject.SubjectSpecificId,
                        SubjectName = subject.Name,
                        NumberOfCredits = subject.NumberOfCredits,
                        TotalLessons = subject.TotalLessons
                    });
                }
                SubjectDisplayList = primarySubjectDisplayList
                    .OrderBy(s => s.SubjectSpecificId)
                    .ToObservableCollection();

                primarySelectedSubjectDisplayList.Clear();
                SelectedSubjectDisplayList = primarySelectedSubjectDisplayList
                    .OrderBy(s => s.SubjectSpecificId)
                    .ToObservableCollection();

                ReloadSubjectsBackground();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error occurred!", "OK");
            }
        }
    }

    private bool CanEndSemesterCommandExecuted()
    {
        return !IsEnd;
    }

    [RelayCommand]
    private async Task AddUpdateSemester()
    {
        if (IsEnd)
        {
            await AddSemester();
        }
        else
        {
            await UpdateSemester();
        }
    }
    #endregion

    #region Property Changed
    partial void OnSelectedSubjectFilterOptionChanged(string value)
    {
        switch (value)
        {
            case "ID":
                SubjectDisplayList = primarySubjectDisplayList
                    .OrderBy(s => s.SubjectSpecificId)
                    .ToObservableCollection();
                break;
            case "Name":
                SubjectDisplayList = primarySubjectDisplayList
                    .OrderBy(b => b.SubjectName)
                    .ToObservableCollection();
                break;
        }

        SearchFilter = "";
        ReloadSubjectsBackground();
    }

    partial void OnSearchFilterChanged(string value)
    {
        switch (SelectedSubjectFilterOption)
        {
            case "ID":
                SubjectDisplayList = primarySubjectDisplayList
                    .Where(s => s.SubjectSpecificId.ToLower().Contains(value.Trim().ToLower()))
                    .OrderBy(s => s.SubjectSpecificId)
                    .ToObservableCollection();
                break;
            case "Name":
                SubjectDisplayList = primarySubjectDisplayList
                    .Where(b => b.SubjectName.ToLower().Contains(value.Trim().ToLower()))
                    .OrderBy(b => b.SubjectName)
                    .ToObservableCollection();
                break;
        }

        ReloadSubjectsBackground();
    }
    #endregion

    #region Helpers
    public void ReloadSubjectsBackground()
    {
        for (int i = 0; i < SubjectDisplayList.Count; i++)
        {
            SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
        }

        SavedSubject = null;
    }

    public void ReloadSelectedSubjectsBackground()
    {
        for (int i = 0; i < SelectedSubjectDisplayList.Count; i++)
        {
            SelectedSubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
        }

        SavedSelectedSubject = null;
    }

    public void SaveSubject(SubjectForAvailableCoursesDiplay subjectForAvailableCoursesDiplay)
    {
        SavedSubject = subjectForAvailableCoursesDiplay;
    }

    public void SaveSelectedSubject(SubjectForAvailableCoursesDiplay subjectForAvailableCoursesDiplay)
    {
        SavedSelectedSubject = subjectForAvailableCoursesDiplay;
    }

    private async Task AddSemester()
    {
        var accept = await Application.Current.MainPage.DisplayAlert("Question", "Are you sure you want to create this new semester?", "Yes", "No");
        if (accept && await ValidateInformation())
        {
            SemesterName semesterNameAttribute = Shared.SemesterName.FirstSemester;
            if (SemesterName == "Second Semester")
            {
                semesterNameAttribute = Shared.SemesterName.SecondSemester;
            }
            else if (SemesterName == "Summer Semester")
            {
                semesterNameAttribute = Shared.SemesterName.SummerSemester;
            }

            Semester semester = new()
            {
                SemesterName = semesterNameAttribute,
                Year = Year,
                MinimumCredits = int.Parse(MinCredit),
                MaximumCredits = int.Parse(MaxCredit),
                StartRegistrationDate = StartDate,
                EndRegistrationDate = EndDate,
                IsEnded = false
            };
            List<int> subjectIds = new();
            foreach (var s in primarySelectedSubjectDisplayList)
            {
                subjectIds.Add(s.Id);
            }

            ISemesterService semesterService = _serviceProvider.GetService<ISemesterService>();
            Semester result = await semesterService.AddSemesterAsync(semester, subjectIds);
            if (result != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "This semester has been created!", "OK");

                IsEnd = false;

                CommandName = "Update semester";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error occurred!", "OK");
            }
        }
    }

    private async Task UpdateSemester()
    {
        var accept = await Application.Current.MainPage.DisplayAlert("Question", "Are you sure you want to update this semester?", "Yes", "No");
        if (accept && await ValidateInformation())
        {
            ISemesterService semesterService = _serviceProvider.GetService<ISemesterService>();
            Semester currentSemester = await semesterService.GetCurrentSemesterAsync();

            SemesterName semesterNameAttribute = Shared.SemesterName.FirstSemester;
            if (SemesterName == "Second Semester")
            {
                semesterNameAttribute = Shared.SemesterName.SecondSemester;
            }
            else if (SemesterName == "Summer Semester")
            {
                semesterNameAttribute = Shared.SemesterName.SummerSemester;
            }

            Semester semester = new()
            {
                Id = currentSemester.Id,
                SemesterName = semesterNameAttribute,
                Year = Year,
                MinimumCredits = int.Parse(MinCredit),
                MaximumCredits = int.Parse(MaxCredit),
                StartRegistrationDate = StartDate,
                EndRegistrationDate = EndDate,
                IsEnded = false
            };
            List<int> subjectIds = new();
            foreach (var s in primarySelectedSubjectDisplayList)
            {
                subjectIds.Add(s.Id);
            }

            bool result = await semesterService.UpdateSemesterAsync(currentSemester.Id, semester, subjectIds);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "This semester has been updated!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Error occurred!", "OK");
            }
        }
    }

    private async Task<bool> ValidateInformation()
    {
        // Validate min credit
        if (string.IsNullOrEmpty(MinCredit))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Min credit is required!", "Ok");
            return false;
        }
        if (!int.TryParse(MinCredit.Trim(), out int minCreditValue) || minCreditValue < 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Min credit is invalid!", "Ok");
            return false;
        }

        // Validate max credit
        if (string.IsNullOrEmpty(MaxCredit))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Max credit is required!", "Ok");
            return false;
        }
        if (!int.TryParse(MaxCredit.Trim(), out int maxCreditValue) || maxCreditValue < 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Max credit is invalid!", "Ok");
            return false;
        }

        // Validate logic of credits
        if (minCreditValue >= maxCreditValue)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Credit values is invalid!", "Ok");
            return false;
        }

        // Validate logic of dates
        if (StartDate >= EndDate)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Start and end date is invalid!", "Ok");
            return false;
        }

        return true;
    }
    #endregion
}
#endregion
