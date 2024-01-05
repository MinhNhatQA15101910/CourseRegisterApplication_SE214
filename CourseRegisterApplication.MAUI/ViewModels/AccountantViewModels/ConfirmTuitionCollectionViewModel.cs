using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;
using CourseRegisterApplication.Shared;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    #region Display
    public partial class TuitionFormDisplay : ObservableObject
    {
        #region Properties

        public ITuitionFormRequester TuitionFormRequester { get; set; }

        public int ID { get; set; }

        [ObservableProperty]
        private string specificId;

        [ObservableProperty]
        private string createdDate;

        [ObservableProperty]
        private TuitionFeeReceiptState state;

        [ObservableProperty]
        private double charge;

        [ObservableProperty]
        private string studentID;

        [ObservableProperty]
        private int courseRegisFormId;

        [ObservableProperty]
        private string tuitionSemester;

        [ObservableProperty]
        private int tuitionSchoolYear;

        [ObservableProperty]
        private Color backgroundColor;



        #endregion

        #region Commands
        [RelayCommand]
        public void DisplayInfomation()
        {
            TuitionFormRequester.ReloadTuitionFormBackground();

            BackgroundColor = Color.FromArgb("#B9D8F2");

            TuitionFormRequester.DisplayInfomation(this);
        }

        #endregion
    }
    #endregion

    #region Requester

    public interface ITuitionFormRequester
    {
        Task DisplayInfomation(TuitionFormDisplay tuitionFormDisplay);

        void ReloadTuitionFormBackground();
    }

    #endregion

    public partial class ConfirmTuitionCollectionViewModel : ObservableObject, ITuitionFormRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IStudentService _studentService;
        #endregion

        #region Properties

        [ObservableProperty]
        private int selectedTuitionFormID = -1;

        [ObservableProperty]
        private string selectedTuitionFormSpecificID;

        [ObservableProperty]
        private int selectedTuitionFormCourseRegisFormID;

        [ObservableProperty]
        private string selectedTuitionFormStudentID;

        [ObservableProperty]
        private string selectedTuitionFormStudentName;

        [ObservableProperty]
        private string selectedTuitionFormCreatedDate;

        [ObservableProperty]
        private double selectedTuitionFormCharge;

        [ObservableProperty]
        private string selectedTuitionSemester;

        [ObservableProperty]
        private int selectedTuitionSchoolYear;

        [ObservableProperty]
        private ObservableCollection<TuitionFormDisplay> tuitionFormDisplaysList = new();

        private readonly List<TuitionFormDisplay> primaryTuitionFormDisplaysList = new();

        [ObservableProperty] private ObservableCollection<string> tutionFormFilterOptions = new() { "Tution ID", "Course ID", "Student ID", "State" };

        [ObservableProperty] private string selectedTutionFormFilterOption = "Tution ID";

        [ObservableProperty] private string searchTutionFormFilter;

        public bool isVisible = false;

        #endregion

        #region Constructors
        public ConfirmTuitionCollectionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _studentService=_serviceProvider.GetService<IStudentService>();
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
        public async Task GetTuitionFormList()
        {
            var tuitionReceiptService = _serviceProvider.GetService<ITuitionFeeReceiptService>();
            var tuitionFormList = await tuitionReceiptService.GetAllTuitionFeeReceipt();
            tuitionFormList = tuitionFormList.Where(c=>c.State==TuitionFeeReceiptState.Pending).ToList();
            await ReloadTuitionFormDisplaysList(tuitionFormList);
        }

        [RelayCommand]
        public async Task TuitionConfirm()
        {
           
            var tuitionFeeReceiptService = _serviceProvider.GetService<ITuitionFeeReceiptService>();
            var tuitionFeeReceipt = await tuitionFeeReceiptService.GetTuitionFeeReceiptsByCourseRegistrationFormId(SelectedTuitionFormCourseRegisFormID);

            var selectedTuitionFeeReceipt = tuitionFeeReceipt.Where(p => p.Id == SelectedTuitionFormID && p.State == TuitionFeeReceiptState.Pending).FirstOrDefault();

            selectedTuitionFeeReceipt.State = TuitionFeeReceiptState.Confirmed;
            selectedTuitionFeeReceipt.CreatedDate = DateTime.Now;

            var result = await tuitionFeeReceiptService.UpdateTuitionFeeReceipt(SelectedTuitionFormID, selectedTuitionFeeReceipt);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Confirm tuition fee receipt successfully", "OK");
                await GetTuitionFormList();
                ClearProperty();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Confirm tuition fee receipt form failed", "OK");
            }
            GetTuitionFormListCommand.Execute(null);
        }

        #endregion

        #region Property Changed

        partial void OnSelectedTutionFormFilterOptionChanged(string value)
        {
            switch (value)
            {
                case "Tution ID":
                    TuitionFormDisplaysList = primaryTuitionFormDisplaysList.OrderBy(p => p.SpecificId).ToObservableCollection();
                    break;
                case "Course ID":
                    TuitionFormDisplaysList = primaryTuitionFormDisplaysList.OrderBy(p => p.CourseRegisFormId).ToObservableCollection();
                    break;
                case "Student ID":
                    TuitionFormDisplaysList = primaryTuitionFormDisplaysList.OrderBy(p => p.StudentID).ToObservableCollection();
                    break;
                case "State":
                    TuitionFormDisplaysList = primaryTuitionFormDisplaysList.OrderBy(p => p.State).ToObservableCollection();
                    break;
            }

            SearchTutionFormFilter = "";
            ReloadTuitionFormBackground();

            ClearProperty();
        }

        partial void OnSearchTutionFormFilterChanged(string value)
        {
            switch (value) 
            { 
            case "Tution ID":
                TuitionFormDisplaysList = primaryTuitionFormDisplaysList.Where(p => p.SpecificId.ToLower().Contains(SearchTutionFormFilter.ToLower())).ToObservableCollection();
                break;
            case "Course ID":
                TuitionFormDisplaysList = primaryTuitionFormDisplaysList.Where(p => p.CourseRegisFormId.ToString().Contains(SearchTutionFormFilter.ToLower())).ToObservableCollection();
                break;
            case "Student ID":
                TuitionFormDisplaysList = primaryTuitionFormDisplaysList.Where(p => p.StudentID.ToString().Contains(SearchTutionFormFilter.ToLower())).ToObservableCollection();
                break;
            case "State":
                TuitionFormDisplaysList = primaryTuitionFormDisplaysList.Where(p => p.State.ToString().Contains(SearchTutionFormFilter.ToLower())).ToObservableCollection();
                break;
            }
        }

        #endregion

        #region Helper

        private async Task ReloadTuitionFormDisplaysList(List<TuitionFeeReceipt> tuitionFeeReceiptsList)
        {
            primaryTuitionFormDisplaysList.Clear();
            var courseRegisFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();
            var semesterService = _serviceProvider.GetService<ISemesterService>();

            if(tuitionFeeReceiptsList.Count > 0)
            {
                foreach(var tuitionFeeReceipt in tuitionFeeReceiptsList)
                {
                    var courseRegisForm = await courseRegisFormService.GetCourseRegistrationFormById(tuitionFeeReceipt.CourseRegistrationFormId);
                    var studentList = await _studentService.GetAllStudents();
                    var semester = await semesterService.GetSemesterById(courseRegisForm.SemesterId);
                    foreach(var item in studentList) 
                    {
                        if (item.Id == courseRegisForm.StudentId)
                        {
                            primaryTuitionFormDisplaysList.Add(new()
                            {
                                TuitionFormRequester = this,
                                ID = tuitionFeeReceipt.Id,
                                SpecificId = tuitionFeeReceipt.TuitionFeeReceiptSpecificId,
                                CreatedDate = tuitionFeeReceipt.CreatedDate.ToString("dd/MM/yyyy"),
                                State = tuitionFeeReceipt.State,
                                Charge = tuitionFeeReceipt.Charge,
                                StudentID = item.StudentSpecificId,
                                TuitionSemester = GetSemesterName(semester.SemesterName.ToString()),
                                TuitionSchoolYear = semester.Year,
                                CourseRegisFormId = tuitionFeeReceipt.CourseRegistrationFormId
                            });
                        }
                    }
                    
                }

                TuitionFormDisplaysList = primaryTuitionFormDisplaysList.OrderBy(p => p.SpecificId).ToObservableCollection();
                ReloadTuitionFormBackground();
            }
        }

        public string GetSemesterName(string semesterName)
        {
            if (semesterName == "FirstSemester") return "Học kỳ I";
            else if (semesterName == "SecondSemester") return "Học kỳ II";
            else return "Học kỳ hè";
        }


        public async Task DisplayInfomation(TuitionFormDisplay tuitionFormDisplay)
        {
            isVisible = true;

            var courseRegisFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();
            var semesterService = _serviceProvider.GetService<ISemesterService>();
            var studentService = _serviceProvider.GetService<IStudentService>();

            var courseRegisForm = await courseRegisFormService.GetCourseRegistrationFormById(tuitionFormDisplay.CourseRegisFormId);
            var semester = await semesterService.GetSemesterById(courseRegisForm.SemesterId);
            var studentList = await studentService.GetAllStudents();

            var student = studentList.Where(p => p.Id == courseRegisForm.StudentId).FirstOrDefault();

            SelectedTuitionFormID = tuitionFormDisplay.ID;

            SelectedTuitionFormSpecificID = tuitionFormDisplay.SpecificId;

            SelectedTuitionFormCourseRegisFormID = tuitionFormDisplay.CourseRegisFormId;

            SelectedTuitionFormStudentID = student.StudentSpecificId;

            SelectedTuitionFormStudentName = student.FullName;

            SelectedTuitionFormCreatedDate = tuitionFormDisplay.CreatedDate;

            SelectedTuitionFormCharge = tuitionFormDisplay.Charge;

            SelectedTuitionSemester = semester.SemesterName.ToString();

            SelectedTuitionSchoolYear = semester.Year;
        }

        public void ReloadTuitionFormBackground()
        {
            for (int i = 0; i < TuitionFormDisplaysList.Count; i++)
            {
                TuitionFormDisplaysList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        void ClearProperty()
        {
            SelectedTuitionFormID = -1;

            SelectedTuitionFormSpecificID = "";

            SelectedTuitionFormCourseRegisFormID = -1;

            SelectedTuitionFormStudentID = "";

            SelectedTuitionFormCreatedDate = "";

            SelectedTuitionFormCharge = 0;

            SelectedTuitionSemester = "";

            SelectedTuitionSchoolYear = 0;
        }

        #endregion
    }
}
