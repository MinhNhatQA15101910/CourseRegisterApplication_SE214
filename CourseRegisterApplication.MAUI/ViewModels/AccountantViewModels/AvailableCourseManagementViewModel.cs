using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;

#region Displays
public partial class SubjectForAvailableCoursesDiplay : ObservableObject
{
    #region Properties
    public ISubjectForAvailableCoursesRequester SubjectForAvailableCoursesRequester { get; set; }

    public string Type { get; set; }

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
    private SubjectForAvailableCoursesDiplay savedSubject = null;

    private SubjectForAvailableCoursesDiplay savedSelectedSubject = null;

    [ObservableProperty] private ObservableCollection<string> subjectFilterOptions = new() { "ID", "Name" };

    [ObservableProperty] private string selectedSubjectFilterOption = "ID";

    private List<Subject> primarySubjectList = new();

    [ObservableProperty] private ObservableCollection<SubjectForAvailableCoursesDiplay> subjectDisplayList = new();

    [ObservableProperty] private ObservableCollection<SubjectForAvailableCoursesDiplay> selectedSubjectDisplayList = new();

    [ObservableProperty] private string semesterName;

    [ObservableProperty] private int year;

    [ObservableProperty] private int minCredit;

    [ObservableProperty] private int maxCredit;

    [ObservableProperty] private DateTime startDate;

    [ObservableProperty] private DateTime endDate;
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

        // Semester is ended case
        if (semester.IsEnded)
        {
            ISubjectService subjectService = _serviceProvider.GetService<ISubjectService>();
            switch (semester.SemesterName)
            {
                case Shared.SemesterName.FirstSemester:
                    SemesterName = "Second Semester";
                    Year = semester.Year;

                    primarySubjectList = (await subjectService.GetSubjectsForSecondSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SecondSemester:
                    SemesterName = "Summer Semester";
                    Year = semester.Year;

                    primarySubjectList = (await subjectService.GetSubjectsForSummerSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SummerSemester:
                    SemesterName = "First Semester";
                    Year = semester.Year + 1;

                    primarySubjectList = (await subjectService.GetSubjectsForFirstSemesterAsync()).ToList();

                    break;
            }

            MinCredit = 0;
            MaxCredit = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            

            SubjectDisplayList.Clear();
            foreach (Subject subject in primarySubjectList)
            {
                SubjectDisplayList.Add(new()
                {
                    SubjectForAvailableCoursesRequester = this, 
                    Type = "Not Selected",
                    SubjectSpecificId = subject.SubjectSpecificId,
                    SubjectName = subject.Name,
                    NumberOfCredits = subject.NumberOfCredits,
                    TotalLessons = subject.TotalLessons
                });
            }

            SelectedSubjectDisplayList.Clear();
        }
        // Semester is not ended case
        else
        {
            ISubjectService subjectService = _serviceProvider.GetService<ISubjectService>();
            switch (semester.SemesterName)
            {
                case Shared.SemesterName.FirstSemester:
                    SemesterName = "First Semester";

                    primarySubjectList = (await subjectService.GetSubjectsForFirstSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SecondSemester:
                    SemesterName = "Second Semester";

                    primarySubjectList = (await subjectService.GetSubjectsForSecondSemesterAsync()).ToList();

                    break;
                case Shared.SemesterName.SummerSemester:
                    SemesterName = "Summer Semester";

                    primarySubjectList = (await subjectService.GetSubjectsForSummerSemesterAsync()).ToList();

                    break;
            }

            Year = semester.Year;
            MinCredit = semester.MinimumCredits;
            MaxCredit = semester.MaximumCredits;
            StartDate = semester.StartRegistrationDate;
            EndDate = semester.EndRegistrationDate;

            List<Subject> subjectListInAvailableCourses = (await subjectService.GetSubjectsBySemesterIdAsync(semester.Id)).OrderBy(s => s.SubjectSpecificId).ToList();

            SelectedSubjectDisplayList.Clear();
            foreach (Subject subject in subjectListInAvailableCourses)
            {
                SelectedSubjectDisplayList.Add(new()
                {
                    SubjectForAvailableCoursesRequester = this,
                    Type = "Selected",
                    SubjectSpecificId = subject.SubjectSpecificId,
                    SubjectName = subject.Name,
                    NumberOfCredits = subject.NumberOfCredits,
                    TotalLessons = subject.TotalLessons
                });
            }

            SubjectDisplayList.Clear();
            foreach (Subject subject in primarySubjectList)
            {
                var subjectsInSelectedListWithSameId = subjectListInAvailableCourses
                    .Where(s => s.Id == subject.Id);

                if (!subjectsInSelectedListWithSameId.Any())
                {
                    SubjectDisplayList.Add(new()
                    {
                        SubjectForAvailableCoursesRequester = this,
                        Type = "Not Selected",
                        SubjectSpecificId = subject.SubjectSpecificId,
                        SubjectName = subject.Name,
                        NumberOfCredits = subject.NumberOfCredits,
                        TotalLessons = subject.TotalLessons
                    });
                }
            }
        }

        ReloadSubjectsBackground();
        ReloadSelectedSubjectsBackground();
    }
    #endregion

    #region Helpers
    public void ReloadSubjectsBackground()
    {
        for (int i = 0; i < SubjectDisplayList.Count; i++)
        {
            SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
        }

        savedSubject = null;
    }

    public void ReloadSelectedSubjectsBackground()
    {
        for (int i = 0; i < SelectedSubjectDisplayList.Count; i++)
        {
            SelectedSubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
        }

        savedSelectedSubject = null;
    }

    public void SaveSubject(SubjectForAvailableCoursesDiplay subjectForAvailableCoursesDiplay)
    {
        savedSubject = subjectForAvailableCoursesDiplay;
    }

    public void SaveSelectedSubject(SubjectForAvailableCoursesDiplay subjectForAvailableCoursesDiplay)
    {
        savedSelectedSubject = subjectForAvailableCoursesDiplay;
    }
    #endregion
}
#endregion
