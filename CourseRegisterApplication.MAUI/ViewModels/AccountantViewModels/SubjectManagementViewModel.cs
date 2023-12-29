using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{

    public partial class SubjectDisplay: ObservableObject
    {
        #region Properties
        public ISubjectRequester SubjectRequester { get; set; }

        [ObservableProperty]
        private string specificId;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string typeName;

        [ObservableProperty]
        private int numberOfCredits;

        [ObservableProperty]
        private int totalLessons;

        [ObservableProperty]
        private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public void DisplaySubjectInformation()
        {
            SubjectRequester.ReloadItemsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            SubjectRequester.DisplaySubjectInformation(this);
        }
        #endregion
    }

    public interface ISubjectRequester
    {
        void ReloadItemsBackground();
        void DisplaySubjectInformation(SubjectDisplay subjectDisplay);
    }

    public partial class SubjectManagementViewModel : ObservableObject, ISubjectRequester
    {

        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        private readonly List<SubjectDisplay> primarySubjectDisplayList = new();

        [ObservableProperty] private ObservableCollection<SubjectDisplay> subjectDisplayList = new();

        [ObservableProperty] private ObservableCollection<string> filterOptions = new() { "ID", "Name", "Type" };

        [ObservableProperty] private string selectedFilterOption = "ID";

        [ObservableProperty] private string searchFilter;

        private int subjectId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSubjectCommand))]
        [NotifyCanExecuteChangedFor(nameof(DisplayUpdateSubjectPopupCommand))]
        private string subjectSpecificId;

        [ObservableProperty] private string subjectName = "Subject:";

        [ObservableProperty] private string subjectType;

        [ObservableProperty] private string totalLesson;

        [ObservableProperty] private string numberOfCredit;
        #endregion

        #region Constructor
        public SubjectManagementViewModel(IServiceProvider serviceProvider)
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
        public async Task GetAllSubject()
        {
            var subjectService = _serviceProvider.GetService<ISubjectService>();
            var subjectList = await subjectService.GetAllSubjects();

            ReloadSubjectDisplays(subjectList);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateSubjectExecuted))]
        public async Task DeleteSubject()
        {
            var subjectService = _serviceProvider.GetService<ISubjectService>();
            var availableCourseService = _serviceProvider.GetService<IAvailableCoursesService>();
            var courseRegistrationDetailService = _serviceProvider.GetService<ICourseRegistrationDetailService>();
            var curriculumService = _serviceProvider.GetService<ICurriculumService>();

            bool accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to delete this subject", "Yes", "No");
            if (accept)
            {
                //if the available course has this subject, display not allow alert
                var availableCourseList = await availableCourseService.GetAvailableCoursesBySubjectId(subjectId);
                if (availableCourseList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because it is in available course!", "OK");
                    return;
                }

                //if there is any course registration detail that has this subject, display not allow alert
                var courseRegistrationDetailList = await courseRegistrationDetailService.GetCourseRegistrationDetailBySubjectId(subjectId);
                if (courseRegistrationDetailList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because there is course registration detail contain its Subject ID!", "OK");
                    return;
                }

                //if there is any curriculum that has this subject, display not allow alert
                var curriculumList = await curriculumService.GetCurriculumsBySubjectId(subjectId);
                if (curriculumList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because there is curriculum contain its Subject ID!", "OK");
                    return;
                }
            }
        }

        [RelayCommand]
        public async Task DisplayAddSubjectPopup()
        {
            var addSubjectPopup = _serviceProvider.GetService<AddSubjectPopup>();
            var addSubjectViewModel = _serviceProvider.GetService<AddSubjectViewModel>();

            addSubjectViewModel.CommandName = "Add Subject";

            await Application.Current.MainPage.ShowPopupAsync(addSubjectPopup);
        }

        [RelayCommand]
        public async Task DisplayUpdateSubjectPopup()
        {
            var addSubjectPopup = _serviceProvider.GetService<AddSubjectPopup>();
            var addSubjectViewModel = _serviceProvider.GetService<AddSubjectViewModel>();

            addSubjectViewModel.CommandName = "Update Subject";

            await Application.Current.MainPage.ShowPopupAsync(addSubjectPopup);
        }

        public bool CanDeleteUpdateSubjectExecuted()
        {
            return !string.IsNullOrEmpty(SubjectSpecificId);
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            if (newValue == "ID")
            {
                SubjectDisplayList = primarySubjectDisplayList.OrderBy(b => b.SpecificId).ToObservableCollection();
            }
            else if (newValue == "Name")
            {
                SubjectDisplayList = primarySubjectDisplayList.OrderBy(b => b.Name).ToObservableCollection();
            }
            else if (newValue == "Type")
            {
                SubjectDisplayList = primarySubjectDisplayList.OrderBy(b => b.TypeName).ToObservableCollection();
            }
            SearchFilter = "";
            ReloadItemsBackground();
            DisplaySubjectInformation(new()
            {
                Name = "",
                SpecificId = "",
                TotalLessons = 0,
                TypeName = "",
                NumberOfCredits = 0,
            });
        }   
        #endregion

        #region Helpers
        private void ReloadSubjectDisplays(List<Subject> subjectList)
        {
            primarySubjectDisplayList.Clear();

            if (subjectList.Count > 0)
            {
                foreach (var subject in subjectList)
                {

                    primarySubjectDisplayList.Add(new()
                    {
                        SubjectRequester = this,
                        SpecificId = subject.SubjectSpecificId,
                        Name = subject.Name,
                    });
                }

                SubjectDisplayList = primarySubjectDisplayList.OrderBy(b => b.SpecificId).ToObservableCollection();
                ReloadItemsBackground();
                DisplaySubjectInformation(new()
                {
                    Name = "",
                    SpecificId = "",
                    TotalLessons = 0,
                    TypeName = "",
                    NumberOfCredits = 0,
                });
            }
        }

        public void ReloadItemsBackground()
        {
            for (int i = 0; i < SubjectDisplayList.Count; i++)
            {
                SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplaySubjectInformation(SubjectDisplay SubjectDisplay)
        {
            SubjectSpecificId = SubjectDisplay.SpecificId;
            SubjectName = SubjectDisplay.Name;
            TotalLesson = SubjectDisplay.TotalLessons.ToString();
            SubjectType = SubjectDisplay.TypeName;
        }
        #endregion
    }
}
