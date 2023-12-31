using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AddSubjectViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string commandName;

        [ObservableProperty] private string subjectSpecificId;

        [ObservableProperty] private Color subjectSpecificIdColor;

        [ObservableProperty] private string subjectSpecificIdMessageText;

        [ObservableProperty] private string subjectName;

        [ObservableProperty] private Color subjectNameColor;

        [ObservableProperty] private string subjectNameMessageText;

        [ObservableProperty] private string subjectType;

        [ObservableProperty] private Color subjectTypeColor;

        [ObservableProperty] private string subjectTypeMessageText;

        [ObservableProperty] private string totalLesson;

        [ObservableProperty] private Color totalLessonColor;

        [ObservableProperty] private string totalLessonMessageText;

        [ObservableProperty]
        private ObservableCollection<SubjectType> subjectTypesList = new();

        #endregion

        #region Constructor
        public AddSubjectViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            //ClearState();

            Popup popup = _serviceProvider.GetService<AddSubjectPopup>();
            await popup.CloseAsync();
        }

        [RelayCommand]
        public async Task GetInformation()
        {
            await Application.Current.MainPage.DisplayAlert("Warning!", "You cannot delete this subject because it is in available course!", "OK");
        }

        /*[RelayCommand(CanExecute = nameof(CanAddUpdateSubjectExecuted))]
        public async Task AddUpdateSubject()
        {
            if (CommandName == "Add Subject")
            {
                await AddSubject();
            }
            else if (CommandName == "Update Subject")
            {
                await UpdateSubject();
            }
        }*/
        public bool CanAddUpdateSubjectExecuted()
        {
            bool isValidSubjectSpecificId = true;
            bool isValidSubjectName = true;

            // Validate Subject ID
            if (string.IsNullOrEmpty(SubjectSpecificId))
            {
                SubjectSpecificIdColor = Color.FromArgb("#BF1D28");
                SubjectSpecificIdMessageText = "Subject ID cannot be empty.";
                isValidSubjectSpecificId = false;
            }
            else
            {
                SubjectSpecificIdColor = Color.FromArgb("#007D3A");
                SubjectSpecificIdMessageText = "Valid Subject ID.";
            }

            // Validate Subject Name
            if (string.IsNullOrEmpty(SubjectName))
            {
                SubjectNameColor = Color.FromArgb("#BF1D28");
                SubjectNameMessageText = "Subject Name cannot be empty.";
                isValidSubjectName = false;
            }
            else
            {
                SubjectNameColor = Color.FromArgb("#007D3A");
                SubjectNameMessageText = "Valid Subject Name.";
            }

            return isValidSubjectSpecificId && isValidSubjectName;
        }

        #endregion

        #region Property Change
        async partial void OnCommandNameChanged(string value)
        {
            if (value == "Add Subject")
            {
                SubjectSpecificId = "";
                SubjectName = "";
                SubjectType = "";
                TotalLesson = "";
            }
            else if (value == "Update Subject")
            {
                SubjectSpecificId = SubjectSpecificId;
                SubjectName = SubjectName;
                SubjectType = SubjectType;
                TotalLesson = TotalLesson;
            }
        }
        #endregion
    }
}
