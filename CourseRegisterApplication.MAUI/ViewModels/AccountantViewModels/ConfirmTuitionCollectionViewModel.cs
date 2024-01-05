using CourseRegisterApplication.MAUI.IServices;
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
        private DateTime createdDate;

        [ObservableProperty]
        private TuitionFeeReceiptState state;

        [ObservableProperty]
        private double charge;

        [ObservableProperty]
        private int studentID;

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
        #endregion

        #region Properties

        [ObservableProperty]
        private int selectedTuitionFormID = -1;

        [ObservableProperty]
        private string selectedTuitionFormSpecificID;

        [ObservableProperty]
        private int selectedTuitionFormCourseRegisFormID;

        [ObservableProperty]
        private int selectedTuitionFormStudentID;

        [ObservableProperty]
        private string selectedTuitionFormStudentName;

        [ObservableProperty]
        private DateTime selectedTuitionFormCreatedDate;

        [ObservableProperty]
        private double selectedTuitionFormCharge;

        [ObservableProperty]
        private string selectedTuitionSemester;

        [ObservableProperty]
        private int selectedTuitionSchoolYear;

        [ObservableProperty]
        private ObservableCollection<TuitionFormDisplay> tuitionFormDisplaysList = new();

        private readonly List<TuitionFormDisplay> primaryTuitionFormDisplaysList = new();

        public bool isVisible = false;

        #endregion

        #region Constructors
        public ConfirmTuitionCollectionViewModel(IServiceProvider serviceProvider)
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
        public async Task GetTuitionFormList()
        {
            var tuitionReceiptService = _serviceProvider.GetService<ITuitionFeeReceiptService>();
            var tuitionFormList = await tuitionReceiptService.GetAllTuitionFeeReceipt();

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
        }

        #endregion

        #region Property Changed
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
                    var semester = await semesterService.GetSemesterById(courseRegisForm.SemesterId);

                    primaryTuitionFormDisplaysList.Add(new()
                    {
                        TuitionFormRequester = this,
                        ID = tuitionFeeReceipt.Id,
                        SpecificId = tuitionFeeReceipt.TuitionFeeReceiptSpecificId,
                        CreatedDate = tuitionFeeReceipt.CreatedDate,
                        State = tuitionFeeReceipt.State,
                        Charge = tuitionFeeReceipt.Charge,
                        StudentID = courseRegisForm.StudentId,
                        TuitionSemester = semester.SemesterName.ToString(),
                        TuitionSchoolYear = semester.Year,
                        CourseRegisFormId = tuitionFeeReceipt.CourseRegistrationFormId
                    });
                }

                TuitionFormDisplaysList = primaryTuitionFormDisplaysList.OrderBy(p => p.SpecificId).ToObservableCollection();
                ReloadTuitionFormBackground();
            }
        }

        public async Task DisplayInfomation(TuitionFormDisplay tuitionFormDisplay)
        {
            isVisible = true;

            var courseRegisFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();
            var semesterService = _serviceProvider.GetService<ISemesterService>();

            var courseRegisForm = await courseRegisFormService.GetCourseRegistrationFormById(tuitionFormDisplay.CourseRegisFormId);
            var semester = await semesterService.GetSemesterById(courseRegisForm.SemesterId);

            SelectedTuitionFormID = tuitionFormDisplay.ID;

            SelectedTuitionFormSpecificID = tuitionFormDisplay.SpecificId;

            SelectedTuitionFormCourseRegisFormID = tuitionFormDisplay.CourseRegisFormId;

            SelectedTuitionFormStudentID = courseRegisForm.StudentId;

            SelectedTuitionFormStudentName = courseRegisForm.Student.FullName;

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

            SelectedTuitionFormStudentID = -1;

            SelectedTuitionFormCreatedDate = DateTime.Today;

            SelectedTuitionFormCharge = 0;

            SelectedTuitionSemester = "";

            SelectedTuitionSchoolYear = 0;
        }

        #endregion
    }
}
