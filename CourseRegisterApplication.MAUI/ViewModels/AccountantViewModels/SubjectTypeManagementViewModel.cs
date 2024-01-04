using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class SubjectTypeDisplay : ObservableObject
    {
        #region Properties

        public ISubjectTypeRequester SubjectTypeRequester { get; set; }
        public int Id { get; set; }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int numberOfLessons;

        [ObservableProperty]
        private double lessonsCharge;

        [ObservableProperty]
        private Color backgroundColor;

        #endregion

        #region Commands
        [RelayCommand]
        public void SelectSubjectType()
        {
            SubjectTypeRequester.ReloadItemsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");
            
            SubjectTypeRequester.GetSubjectTypeInformation(this);
        }
        #endregion
    }

    public interface ISubjectTypeRequester
    {
        void ReloadItemsBackground();
        void GetSubjectTypeInformation(SubjectTypeDisplay subjectTypeDisplay);
    }

    public partial class SubjectTypeManagementViewModel : ObservableObject, ISubjectTypeRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubjectTypeService _subjectTypeService;
        #endregion

        #region Properties
        private readonly List<SubjectTypeDisplay> primarySubjectTypeDisplayList = new();

        [ObservableProperty] private ObservableCollection<SubjectTypeDisplay> subjectTypeDisplayList = new();

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int numberOfLessons;

        private int selectedSubjectTypeId;

        [ObservableProperty]
        private double lessonsCharge;

        public ISubjectTypeRequester SubjectTypeRequester { get; set; }

        #endregion

        #region Constructor
        public SubjectTypeManagementViewModel(IServiceProvider serviceProvider)
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
        public async Task GetAllSubjectType()
        {
            var subjectTypeList = await _subjectTypeService.GetAllSubjectType();

            ReloadSubjectTypeDisplays(subjectTypeList);
        }

        [RelayCommand]
        public async Task DeleteSubjectType()
        {
            var subjectService = _serviceProvider.GetService<ISubjectService>();

            bool accept = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to delete this subject type?", "Yes", "No");
            if (accept)
            {
                var subjectList = await subjectService.GetAllSubjects();
                subjectList = subjectList.Where(s => s.SubjectTypeId == selectedSubjectTypeId).ToList();
                if (subjectList.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "This subject type is being used by some subjects", "OK");
                }

                var result = await subjectService.DeleteSubject(selectedSubjectTypeId);
                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Delete subject type successfully!", "OK");
                    GetAllSubjectTypeCommand.Execute(null);
                }
            }
            
        }

        [RelayCommand]
        public async Task DisplayAddSubjectTypePopup()
        {
            var addUpdateSubjectTypePopup = _serviceProvider.GetService<AddUpdateSubjectTypePopup>();
            var addUpdateSubjectTypeViewModel = _serviceProvider.GetService<AddUpdateSubjectTypeViewModel>();

            addUpdateSubjectTypeViewModel.CommandName = "Add subject type";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateSubjectTypePopup);
        }

        [RelayCommand]
        public async Task DisplayUpdateSubjectPopup()
        {
            var addUpdateSubjectTypePopup = _serviceProvider.GetService<AddUpdateSubjectTypePopup>();
            var addUpdateSubjectTypeViewModel = _serviceProvider.GetService<AddUpdateSubjectTypeViewModel>();

            addUpdateSubjectTypeViewModel.SubjectTypeId = selectedSubjectTypeId;
            addUpdateSubjectTypeViewModel.CommandName = "Update subject type";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateSubjectTypePopup);
        }


        #endregion

        #region Helpers
        private void ReloadSubjectTypeDisplays(List<SubjectType> subjectTypeList)
        {
            primarySubjectTypeDisplayList.Clear();
            if (subjectTypeList.Count > 0)
            {
                foreach (var subjectType in subjectTypeList)
                {
                    primarySubjectTypeDisplayList.Add(new()
                    {
                        SubjectTypeRequester = this,
                        Id = subjectType.Id,
                        Name = subjectType.Name,
                        NumberOfLessons = subjectType.NumberOfLessons,
                        LessonsCharge = subjectType.LessonsCharge
                    });
                }

                SubjectTypeDisplayList = primarySubjectTypeDisplayList.OrderBy(b => b.Name).ToObservableCollection();
                ReloadItemsBackground();
            }
        }
        public void ReloadItemsBackground()
        {
            for (int i = 0; i < SubjectTypeDisplayList.Count; i++)
            {
                SubjectTypeDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void GetSubjectTypeInformation(SubjectTypeDisplay subjectTypeDisplay)
        {
            selectedSubjectTypeId = subjectTypeDisplay.Id;
        }
        #endregion
    }
}
