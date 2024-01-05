using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class StatisticDisplay : ObservableObject
    {
        #region Properties
        public IStatisticRequester StatisticRequester { get; set; }

        [ObservableProperty]
        private int cRFId;

        [ObservableProperty]
        private string studentID;

        [ObservableProperty]
        private string studentName;

        [ObservableProperty]
        private string totalTuition;

        [ObservableProperty]
        private string actualTuition;

        [ObservableProperty]
        private string paidTuition;

        [ObservableProperty]
        private string remainTuition;

        [ObservableProperty]
        private Color statisticBackgroundColor;
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseIStatistic()
        {
            StatisticRequester.ReloadStatictisItemsBackground();
            StatisticBackgroundColor = Color.FromArgb("#B9D8F2");
            StatisticRequester.ChooseStatistic(this);

        }
        #endregion
    }

    #region IRequesters
    public interface IStatisticRequester
    {
        void ReloadStatictisItemsBackground();
        void ChooseStatistic(StatisticDisplay statisticDisplay);
    }
    #endregion

    public partial class UnpayTuitionStudentsReportViewModel : ObservableObject, IStatisticRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly IStudentService _studentService;
        private readonly ICourseRegistrationFormService _courseRegistrationFormService;
        private readonly ITuitionFeeReceiptService _tuitionFeeReceiptService;
        private readonly ISemesterService _semester;
        #endregion

        #region Constructor
        public UnpayTuitionStudentsReportViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _studentService = _serviceProvider.GetService<IStudentService>();
            _courseRegistrationFormService = _serviceProvider.GetService<ICourseRegistrationFormService>();
            _tuitionFeeReceiptService = _serviceProvider.GetService<ITuitionFeeReceiptService>();
            _semester = _serviceProvider.GetService<ISemesterService>();
        }
        #endregion

        #region Properties
        List<int> schoolYearList = new List<int>();
        List<string> semesterNameList = new List<string>();
        List<CourseRegistrationForm> courseRegistrationFormList=new List<CourseRegistrationForm>();
        List<TuitionFeeReceipt> tuitionFeeReceiptList = new List<TuitionFeeReceipt>();
        List<Semester> semesterList = new List<Semester>();
        List<Student> studentList = new List<Student>();
        int selectedCRFId = 0;

        private readonly List<StatisticDisplay> primaryStatisticDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<StatisticDisplay> statisticDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<string> schoolYearFilterOptions = new();

        [ObservableProperty]
        private string selectedSchoolYearFilterOption = "";

        [ObservableProperty]
        private ObservableCollection<string> semesterFilterOptions = new();

        [ObservableProperty]
        private string selectedSemesterFilterOption = "";

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
        public async Task GetSetUp()
        {
            semesterList = await _semester.GetAllSemester();
            courseRegistrationFormList = await _courseRegistrationFormService.GetAllCourseRegistrationForm();
            studentList = await _studentService.GetAllStudents();
            tuitionFeeReceiptList = await _tuitionFeeReceiptService.GetAllTuitionFeeReceipt();
            schoolYearList = semesterList.Select(item => item.Year).Distinct().OrderByDescending(year => year).ToList();
            semesterNameList = semesterList.Select(item => item.SemesterName.ToString()).Distinct().OrderBy(name => name).ToList();

            if (schoolYearList.Count > 0)
            {
                SchoolYearFilterOptions.Clear();
                foreach (var item in schoolYearList)
                {
                    SchoolYearFilterOptions.Add(item.ToString());
                }
                SelectedSchoolYearFilterOption = SchoolYearFilterOptions[0];
            }
        }

        
        #endregion

        #region Property Changed
        partial void OnSelectedSchoolYearFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && newValue != null)
            {
                SemesterFilterOptions.Clear();
                semesterNameList.Clear();
                foreach (var item in semesterList)
                {
                    if (item.Year == Int32.Parse(newValue.ToString()))
                    {
                        semesterNameList.Add(item.SemesterName.ToString());
                    }
                }
                foreach (var item in semesterNameList)
                {
                    SemesterFilterOptions.Add(SetUpSemesterName(item));
                }
                SelectedSemesterFilterOption = SemesterFilterOptions[0];
            }

        }

        partial void OnSelectedSemesterFilterOptionChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && newValue != null)
            {
                int currentSchoolYear = Int32.Parse(SelectedSchoolYearFilterOption);
                List<CourseRegistrationForm> currentCourseRegistrationFormList = courseRegistrationFormList
                    .Where(item =>
                        semesterList.Exists(item2 =>
                            item.SemesterId == item2.Id &&
                            item2.SemesterName.ToString() == newValue &&
                            item2.Year == currentSchoolYear))
                    .ToList();
                if (currentCourseRegistrationFormList.Count > 0)
                {
                    ReloadStatisticDisplays(currentCourseRegistrationFormList);
                }
            }
        }
        #endregion

        #region Helper

        private void ReloadStatisticDisplays(List<CourseRegistrationForm> courseRegistrationFormList)
        {
            primaryStatisticDisplayList.Clear();
            StatisticDisplayList = null;

            if (courseRegistrationFormList.Count > 0)
            {
                primaryStatisticDisplayList.AddRange(
                from item in courseRegistrationFormList
                join item2 in tuitionFeeReceiptList on item.Id equals item2.CourseRegistrationFormId
                join item3 in studentList on item.StudentId equals item3.Id
                where item.State == CourseRegistrationFormState.Expired
                select new StatisticDisplay
                {
                    CRFId = item.Id,
                    StudentID = item3.StudentSpecificId,
                    StudentName = item3.FullName,
                    TotalTuition = item.TotalCharge.ToString(),
                    ActualTuition = item.TotalChargeWithDiscount.ToString(),
                    PaidTuition = CalculatePaidTuition(item.Id).ToString(),
                    RemainTuition = (item.TotalChargeWithDiscount - CalculatePaidTuition(item.Id)).ToString(),
                });
                StatisticDisplayList = primaryStatisticDisplayList
                    .Where(d => double.TryParse(d.RemainTuition, out double remainTuition) && remainTuition > 0)
                    .OrderBy(d => d.StudentID).ToObservableCollection();
                ReloadStatictisItemsBackground();
            }
        }

        public double CalculatePaidTuition(int courseRegistrationFormId)
        {
            double paidTuition = tuitionFeeReceiptList
                .Where(item => item.CourseRegistrationFormId == courseRegistrationFormId)
                .Sum(item => item.Charge);
            paidTuition = Math.Round(paidTuition);
            return paidTuition;
        }

        public string SetUpSemesterName(string semesterName)
        {
            if (semesterName == "FirstSemester")
            {
                return "Học kỳ I";
            }
            else if (semesterName == "SecondSemester")
            {
                return "Học kỳ II";
            }
            return "Học kỳ hè";
        }
        #endregion
        public void ReloadStatictisItemsBackground()
        {
            for (int i = 0; i < StatisticDisplayList.Count; i++)
            {
                StatisticDisplayList[i].StatisticBackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void ChooseStatistic(StatisticDisplay statisticDisplay)
        {
            selectedCRFId = statisticDisplay.CRFId;
        }

    }
}
