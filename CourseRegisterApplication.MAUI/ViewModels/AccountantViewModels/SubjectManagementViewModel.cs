using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{

    public partial class SubjectDisplay: ObservableObject
    {
        #region Properties
        public ISubjectRequester SubjectRequester { get; set; }

        public int SubjectId { get; set; }

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
        private readonly ISubjectTypeService _subjectTypeService;
        #endregion

        #region Properties
        private readonly List<SubjectDisplay> primarySubjectDisplayList = new();

        [ObservableProperty] private ObservableCollection<SubjectDisplay> subjectDisplayList = new();

        [ObservableProperty] private ObservableCollection<string> filterOptions = new() { "ID", "Name", "Type" };

        [ObservableProperty] private string selectedFilterOption = "ID";

        [ObservableProperty] private string searchFilter;

        private int selectedSubjectId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSubjectCommand))]
        [NotifyCanExecuteChangedFor(nameof(DisplayUpdateSubjectPopupCommand))]
        private string selectedSubjectSpecificId;

        [ObservableProperty] private string selectedSubjectName;

        [ObservableProperty] private string selectedSubjectType;

        [ObservableProperty] private string selectedTotalLesson;

        [ObservableProperty] private string selectedNumberOfCredit;

        public ISubjectRequester SubjectRequester { get; set; }
        #endregion

        #region Constructor
        public SubjectManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _subjectTypeService = _serviceProvider.GetRequiredService<ISubjectTypeService>();
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
            var subjectList = await subjectService.GetAllSubject();

            await ReloadSubjectDisplays(subjectList);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateSubjectExecuted))]
        public async Task DeleteSubject()
        {
            var subjectService = _serviceProvider.GetService<ISubjectService>();
            var availableCourseService = _serviceProvider.GetService<IAvailableCourseService>();
            var courseRegistrationDetailService = _serviceProvider.GetService<ICourseRegistrationDetailService>();
            var curriculumService = _serviceProvider.GetService<ICurriculumService>();

            bool accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to delete this subject", "Yes", "No");
            if (accept)
            {
                //if the available course has this subject, display not allow alert
                var availableCourseList = await availableCourseService.GetAvailableCourseBySubjectId(selectedSubjectId);
                if (availableCourseList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because it is in available course!", "OK");
                    return;
                }

                //if there is any course registration detail that has this subject, display not allow alert
                var courseRegistrationDetailList = await courseRegistrationDetailService.GetCourseRegistrationDetailBySubjectId(selectedSubjectId);
                if (courseRegistrationDetailList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because there is course registration detail contain its Subject ID!", "OK");
                    return;
                }

                //if there is any curriculum that has this subject, display not allow alert
                var curriculumList = await curriculumService.GetCurriculumsBySubjectId(selectedSubjectId);
                if (curriculumList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because there is curriculum contain its Subject ID!", "OK");
                    return;
                }

                //delete subject
                var result = await subjectService.DeleteSubject(selectedSubjectId);
                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Delete subject successfully!", "OK");
                    GetAllSubjectCommand.Execute(null);
                }
            }
        }

        [RelayCommand]
        public async Task DisplayAddSubjectPopup()
        {
            var addSubjectPopup = _serviceProvider.GetService<AddUpdateSubjectPopup>();
            var addSubjectViewModel = _serviceProvider.GetService<AddUpdateSubjectViewModel>();

            addSubjectViewModel.CommandName = "Add Subject";

            await Application.Current.MainPage.ShowPopupAsync(addSubjectPopup);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateSubjectExecuted))]
        public async Task DisplayUpdateSubjectPopup()
        {
            var addUpdateSubjectPopup = _serviceProvider.GetService<AddUpdateSubjectPopup>();
            var addUpdateSubjectViewModel = _serviceProvider.GetService<AddUpdateSubjectViewModel>();

            addUpdateSubjectViewModel.SubjectId = selectedSubjectId;
            addUpdateSubjectViewModel.CommandName = "Update Subject";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateSubjectPopup);
        }

        public bool CanDeleteUpdateSubjectExecuted()
        {
            return !string.IsNullOrEmpty(SelectedSubjectSpecificId);
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
            ResetDisplaySubjectInformation();
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            switch(SelectedFilterOption)
            {
                case "ID":
                    SubjectDisplayList = primarySubjectDisplayList.Where(b => b.SpecificId.ToLower().Contains(newValue.ToLower())).OrderBy(a => a.SpecificId).ToObservableCollection();
                    break;
                case "Name":
                    SearchSubjectByName(newValue);
                    break;
                case "Type":
                    SearchSubjectByTypeName(newValue);
                    break;
            }
        }
        #endregion

        #region Helpers
        private async Task ReloadSubjectDisplays(List<Subject> subjectList)
        {
            primarySubjectDisplayList.Clear();
            if (subjectList.Count > 0)
            {
                foreach (var subject in subjectList)
                {
                    var subjectType = await _subjectTypeService.GetSubjectTypeById(subject.SubjectTypeId);
                    primarySubjectDisplayList.Add(new()
                    {
                        SubjectRequester = this,
                        SubjectId = subject.Id,
                        SpecificId = subject.SubjectSpecificId,
                        Name = subject.Name,
                        TypeName = subjectType.Name,
                        TotalLessons = subject.TotalLessons,
                        NumberOfCredits = subject.NumberOfCredits,
                    });
                }

                SubjectDisplayList = primarySubjectDisplayList.OrderBy(b => b.SpecificId).ToObservableCollection();
                ReloadItemsBackground();
                ResetDisplaySubjectInformation();
            }
        }

        public void ReloadItemsBackground()
        {
            for (int i = 0; i < SubjectDisplayList.Count; i++)
            {
                SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplaySubjectInformation(SubjectDisplay subjectDisplay)
        {
            selectedSubjectId = subjectDisplay.SubjectId;
            SelectedSubjectSpecificId = subjectDisplay.SpecificId;
            SelectedSubjectName = subjectDisplay.Name;
            SelectedTotalLesson = subjectDisplay.TotalLessons.ToString();
            SelectedSubjectType = subjectDisplay.TypeName;
            SelectedNumberOfCredit = subjectDisplay.NumberOfCredits.ToString();
        }

        private void ResetDisplaySubjectInformation()
        {
            DisplaySubjectInformation(new SubjectDisplay()
            {
                SubjectId = 0,
                SpecificId = "",
                Name = "",
                TypeName = "",
                TotalLessons = 0,
                NumberOfCredits = 0
            });
        }

        static string RemoveAccents(string input)
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

        void SearchSubjectByName(string newValue)
        {
            string normalizedValue = SubjectManagementViewModel.RemoveAccents(newValue);

            string pattern = string.Join(".*", normalizedValue.Select(c => Regex.Escape(c.ToString())));
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            SubjectDisplayList = primarySubjectDisplayList
                .Where(a => regex.IsMatch(SubjectManagementViewModel.RemoveAccents(a.Name)))
                .OrderBy(a => a.Name)
                .ToObservableCollection();
        }

        void SearchSubjectByTypeName(string newValue)
        {
            string normalizedValue = SubjectManagementViewModel.RemoveAccents(newValue);

            string pattern = string.Join(".*", normalizedValue.Select(c => Regex.Escape(c.ToString())));
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            SubjectDisplayList = primarySubjectDisplayList
                .Where(a => regex.IsMatch(SubjectManagementViewModel.RemoveAccents(a.TypeName)))
                .OrderBy(a => a.Name)
                .ToObservableCollection();
        }
        #endregion
    }
}
