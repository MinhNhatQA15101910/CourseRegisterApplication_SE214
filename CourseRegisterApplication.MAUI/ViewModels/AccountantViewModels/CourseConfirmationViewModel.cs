using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class CourseRegisFormDisplay: ObservableObject
    {
        #region Properties

        public ICourseRegisFormRequester CourseRegisFormRequester { get; set; }

        public int ID { get; set; }

        [ObservableProperty]
        private string specificId;

        [ObservableProperty]
        private DateTime createdDate;

        [ObservableProperty]
        private CourseRegistrationFormState state;

        [ObservableProperty]
        private int studentId;

        [ObservableProperty]
        private int semesterId;

        [ObservableProperty]
        private Color backgroundColor;

        #endregion

        #region Commands
        [RelayCommand]
        public void DisplayCourseRegisDetailInfomation()
        {
            CourseRegisFormRequester.ReloadCourseRegisFormBackground();

            BackgroundColor = Color.FromArgb("#B9D8F2");

            CourseRegisFormRequester.DisplayInformation(this);

            CourseRegisFormRequester.DisplayCourseRegisDetailList(this);
        }
        #endregion
    }

    public partial class CourseRegisDetailDisplay: ObservableObject
    {
        #region Properties
        public ICourseRegisDetailRequester CourseRegisDetailRequester { get; set; }

        [ObservableProperty]
        private int courseRegisFormId;

        [ObservableProperty]
        private int subjectId;

        [ObservableProperty]
        private string subjectName;

        [ObservableProperty]
        private string subjectType;

        [ObservableProperty]
        private int numberOfPeriod;

        [ObservableProperty]
        private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public void DisplayInfomation()
        {
            CourseRegisDetailRequester.ReloadCourseRegisDetailBackground();

            BackgroundColor = Color.FromArgb("#B9D8F2");
        }
        #endregion
    }

    #region Requester
    public interface ICourseRegisFormRequester
    {
        void DisplayCourseRegisDetailList(CourseRegisFormDisplay courseRegisFormDisplay);
        Task DisplayInformation(CourseRegisFormDisplay courseRegisFormDisplay);
        void ReloadCourseRegisFormBackground();
    }

    public interface ICourseRegisDetailRequester
    {
        void ReloadCourseRegisDetailBackground();
    }
    #endregion

    public partial class CourseConfirmationViewModel : ObservableObject, ICourseRegisFormRequester, ICourseRegisDetailRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty]
        private string selectedFormSpecificId;

        [ObservableProperty]
        private string selectedFormStudentId;

        [ObservableProperty]
        private string selectedFormStudentName;

        [ObservableProperty]
        private DateTime selectedFormCreatedDate;

        [ObservableProperty]
        private string selectedFormState;

        [ObservableProperty]
        private string selectedFormSemesterName;

        [ObservableProperty]
        private string selectedFormSemesterYear;

        [ObservableProperty]
        private int selectedFormId = -1;

        [ObservableProperty] private ObservableCollection<string> courseRegisFormFilterOptions = new() { "Form ID", "Student ID" };

        [ObservableProperty] private string selectedCourseRegisFormFilterOption = "Form ID";

        [ObservableProperty] private string searchCourseRegisFormFilter;

        private readonly List<CourseRegisFormDisplay> primaryCourseRegisFormDisplayList = new();

        [ObservableProperty] private ObservableCollection<CourseRegisFormDisplay> courseRegisFormDisplayList = new();

        private readonly List<CourseRegisDetailDisplay> primaryCourseRegisDetailDisplayList = new();

        [ObservableProperty] private ObservableCollection<CourseRegisDetailDisplay> courseRegisDetailDisplayList = new();
        #endregion

        #region Constructors
        public CourseConfirmationViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task Logout()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (accept)
            {
                Application.Current.MainPage = _serviceProvider.GetService<LoginPage>();
            }
        }

        [RelayCommand]
        public async Task GetCourseRegisForm()
        {
            var courseRegisFormService = _serviceProvider.GetRequiredService<ICourseRegistrationFormService>();
            var courseRegisFormList = await courseRegisFormService.GetAllCourseRegistrationFormsWithPendingState();

            ReloadCourseRegisFormDisplay(courseRegisFormList);
        }

        [RelayCommand]
        public async Task ConfirmCourseRegisForm()
        {

            var courseRegisFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();
            var courseRegisForm = await courseRegisFormService.GetAllCourseRegistrationFormsWithPendingState();

            var selectCourseRegisForm = courseRegisForm.Where(p => p.Id == SelectedFormId).FirstOrDefault();

            selectCourseRegisForm.State = CourseRegistrationFormState.Confirmed;
            selectCourseRegisForm.CreatedDate = DateTime.Now;

            var result = await courseRegisFormService.UpdateCourseRegistrationForm(SelectedFormId ,selectCourseRegisForm);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Confirm course registration form successfully", "OK");
                await GetCourseRegisForm();
                ClearProperty();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Confirm course registration form failed", "OK");
            }
            GetCourseRegisFormCommand.Execute(null);
        }
        #endregion

        #region Property Changed

        partial void OnSelectedCourseRegisFormFilterOptionChanged(string value)
        {
            switch (value)
            {
                case "Form ID":
                    CourseRegisFormDisplayList = primaryCourseRegisFormDisplayList.OrderBy(p => p.SpecificId).ToObservableCollection();
                    break;
                case "Student ID":
                    CourseRegisFormDisplayList = primaryCourseRegisFormDisplayList.OrderBy(p => p.StudentId).ToObservableCollection();
                    break;
            }

            SearchCourseRegisFormFilter = "";
            ReloadCourseRegisFormBackground();

            ClearProperty();
        }

        partial void OnSearchCourseRegisFormFilterChanged(string value)
        {
            switch (value)
            {
                case "Form ID":
                    CourseRegisFormDisplayList = primaryCourseRegisFormDisplayList.Where(p => p.SpecificId.ToLower().Contains(SearchCourseRegisFormFilter.ToLower())).ToObservableCollection();
                    break;
                case "Student ID":
                    CourseRegisFormDisplayList = primaryCourseRegisFormDisplayList.Where(p => p.StudentId.ToString().ToLower().Contains(SearchCourseRegisFormFilter.ToLower())).ToObservableCollection();
                    break;
            }

            ReloadCourseRegisFormBackground();
        }

        #endregion

        #region Helper

        private void ReloadCourseRegisFormDisplay(List<CourseRegistrationForm> courseRegisFormList)
        {
            var courseRegisFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();

            primaryCourseRegisFormDisplayList.Clear();

            if(courseRegisFormList.Count > 0)
            {
                foreach(var courseRegisForm in courseRegisFormList)
                {
                    primaryCourseRegisFormDisplayList.Add(new()
                    {
                        CourseRegisFormRequester = this,
                        ID = courseRegisForm.Id,
                        SpecificId = courseRegisForm.CourseRegistrationFormSpecificId,
                        CreatedDate = courseRegisForm.CreatedDate,
                        State = courseRegisForm.State,
                        StudentId = courseRegisForm.StudentId,
                        SemesterId = courseRegisForm.SemesterId,
                    });
                }

                CourseRegisFormDisplayList = primaryCourseRegisFormDisplayList.OrderBy(p => p.SpecificId).ToObservableCollection();
                ReloadCourseRegisFormBackground();


            }
        }

        public void ReloadCourseRegisFormBackground()
        {
            for (int i = 0; i < CourseRegisFormDisplayList.Count; i++)
            {
                CourseRegisFormDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        private async Task ReloadCourseRegisDetailDisplay(List<CourseRegistrationDetail> courseRegisDetailList)
        {
            primaryCourseRegisDetailDisplayList.Clear();

            var subjectService = _serviceProvider.GetService<ISubjectService>();
            var subjectTypeService = _serviceProvider.GetService<ISubjectTypeService>();

            if (courseRegisDetailList.Count > 0)
            {
                foreach (var courseRegisDetail in courseRegisDetailList)
                {
                    var subject = await subjectService.GetSubjectById(courseRegisDetail.SubjectId);
                    var subjectType = await subjectTypeService.GetSubjectTypeById(subject.SubjectTypeId);

                    primaryCourseRegisDetailDisplayList.Add(new()
                    {
                        CourseRegisDetailRequester = this,
                        CourseRegisFormId = courseRegisDetail.CourseRegistrationFormId,
                        SubjectId = courseRegisDetail.SubjectId,
                        SubjectName = subject.Name,
                        SubjectType = subjectType.Name,
                        NumberOfPeriod = subject.TotalLessons
                    }) ;
                }

                CourseRegisDetailDisplayList = primaryCourseRegisDetailDisplayList.OrderBy(p => p.CourseRegisFormId).ToObservableCollection();
                ReloadCourseRegisDetailBackground();

                
            }
        }

        public async Task DisplayInformation(CourseRegisFormDisplay courseRegisFormDisplay)
        {
            var studentService = _serviceProvider.GetService<IStudentService>();
            var semesterService = _serviceProvider.GetService<ISemesterService>();

            var studentList = await studentService.GetAllStudents();
            var selectedStudent = studentList.Where(p => p.Id == courseRegisFormDisplay.StudentId).FirstOrDefault();

            var selectedSemester = await semesterService.GetSemesterById(courseRegisFormDisplay.SemesterId);

            SelectedFormId = courseRegisFormDisplay.ID;
            SelectedFormSpecificId = courseRegisFormDisplay.SpecificId;
            SelectedFormStudentId = selectedStudent.StudentSpecificId;
            SelectedFormCreatedDate = courseRegisFormDisplay.CreatedDate;
            SelectedFormState = courseRegisFormDisplay.State.ToString();
            SelectedFormSemesterName = selectedSemester.SemesterName.ToString();
            SelectedFormSemesterYear = selectedSemester.Year.ToString();
            SelectedFormStudentName = selectedStudent.FullName;

        }

        public void ReloadCourseRegisDetailBackground()
        {
            for (int i = 0; i < CourseRegisFormDisplayList.Count; i++)
            {
                CourseRegisDetailDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public async void DisplayCourseRegisDetailList(CourseRegisFormDisplay courseRegisFormDisplay)
        {
            SelectedFormId = courseRegisFormDisplay.ID;

            var courseRegisDetailService = _serviceProvider.GetService<ICourseRegistrationDetailService>();
            var courseRegisDetailList = await courseRegisDetailService.GetCRDByCRFId(SelectedFormId);

            await ReloadCourseRegisDetailDisplay(courseRegisDetailList);

        }

        void ClearProperty()
        {
            SelectedFormId = -1;
            SelectedFormSpecificId = "";
            SelectedFormStudentId = "";
            SelectedFormStudentName = "";
            //SelectedFormCreatedDate = "";
            SelectedFormState = "";
            SelectedFormSemesterName = "";
        }

        #endregion

    }
}
