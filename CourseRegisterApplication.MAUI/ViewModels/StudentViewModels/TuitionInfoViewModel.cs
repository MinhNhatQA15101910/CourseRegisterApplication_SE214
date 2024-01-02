using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.ViewModels.Displays;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.Shared;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class TuitionInfoViewModel : ObservableObject, ICourseRegistrationRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly ITuitionFeeReceiptService _tuitionFeeReceiptService;
        private readonly ICourseRegistrationFormService _courseRegistrationFormService;
        private readonly ICourseRegistrationDetailService _courseRegistrationDetailService;
        private readonly ISubjectService _subjectService;
        private readonly ISubjectTypeService _subjectTypeService;
        private readonly ISemesterService _semesterService;
        private readonly IStudentService _studentService;
        private readonly IPriorityTypeService _priorityTypeService;
        private readonly IStudentPriorityTypeService _studentPriorityTypeService;
        #endregion

        #region Constructor
        public TuitionInfoViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _tuitionFeeReceiptService = _serviceProvider.GetRequiredService<ITuitionFeeReceiptService>();
            _courseRegistrationFormService = _serviceProvider.GetRequiredService<ICourseRegistrationFormService>();
            _courseRegistrationDetailService = _serviceProvider.GetRequiredService<ICourseRegistrationDetailService>();
            _subjectService = _serviceProvider.GetRequiredService<ISubjectService>();
            _subjectTypeService = _serviceProvider.GetRequiredService<ISubjectTypeService>();
            _semesterService = _serviceProvider.GetRequiredService<ISemesterService>();
            _studentService = _serviceProvider.GetRequiredService<IStudentService>();
            _priorityTypeService = _serviceProvider.GetRequiredService<IPriorityTypeService>();
            _studentPriorityTypeService = _serviceProvider.GetRequiredService<IStudentPriorityTypeService>();
        }
        #endregion

        #region Properties
        private readonly List<CourseRegistrationDisplay> primaryCourseRegistrationDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<CourseRegistrationDisplay> courseRegistrationDisplayList = new();

        [ObservableProperty]
        private string currentSemester;

        [ObservableProperty]
        private int currentCourseRegistrationId;

        [ObservableProperty]
        private double currentTotalTuition;

        [ObservableProperty]
        private double currentPaidTuition;

        [ObservableProperty]
        private string currentSchoolYear;

        [ObservableProperty]
        private string currentLastPaidTuition;

        [ObservableProperty]
        private double currentRealityTution;
        
        [ObservableProperty]
        private double currentRemainTution;

        [ObservableProperty]
        private string toolTipRealityTuition; 

        [ObservableProperty]
        private bool isVisibleTuitionInfo;

        List<Student> studentList = new List<Student>();
        List<PriorityType> priorityTypeList = new List<PriorityType>();
        List<StudentPriorityType> studentPriorityTypeList = new List<StudentPriorityType>();
        List<Subject> subjectList = new List<Subject>();
        List<Semester> semesterList = new List<Semester>();
        List<CourseRegistrationForm> courseRegistrationFormList = new List<CourseRegistrationForm>();
        List<TuitionFeeReceipt> tuitionFeeReceiptList = new List<TuitionFeeReceipt>();

        int selectedCourseRegistrationId;
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
        public async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand]
        public async Task GetSetUp()
        {
            studentList = await _studentService.GetStudents();
            Student thisStudent = studentList.Find(item => item.StudentSpecificId == GlobalConfig.CurrentUser.Username);
            if (thisStudent != null)
            {
                courseRegistrationFormList = await _courseRegistrationFormService.GetCourseRegistrationFormByStudentId(thisStudent.Id);
                tuitionFeeReceiptList=await _tuitionFeeReceiptService.GetAllTuitionFeeReceipt();
                subjectList = await _subjectService.GetAllSubjects();
                semesterList = await _semesterService.GetAllSemester();
                priorityTypeList = await _priorityTypeService.GetAllPriorityType();
                studentPriorityTypeList = await _studentPriorityTypeService.GetStudentPriorityTypesByStudentId(thisStudent.Id);
                IsVisibleTuitionInfo = false;

                ReloadCourseRegistrationDisplays(courseRegistrationFormList);
            }
        }

        #endregion
        #region Property Chaged
        public async Task ChooseCourseRegistration(CourseRegistrationDisplay courseRegistrationDisplay)
        {
            selectedCourseRegistrationId = courseRegistrationDisplay.CourseRegistrationId;
            List<TuitionFeeReceipt> thisTuitionFeeReceiptList = await _tuitionFeeReceiptService.GetTuitionFeeReceiptsByCourseRegistrationFormId(selectedCourseRegistrationId);
            if(thisTuitionFeeReceiptList != null)
            {
                IsVisibleTuitionInfo = true;
                CurrentSemester = courseRegistrationDisplay.CourseRegistrationSemester;
                CurrentSchoolYear = courseRegistrationDisplay.CourseRegistrationSchoolYear;
                CurrentCourseRegistrationId = courseRegistrationDisplay.CourseRegistrationId;
                CurrentLastPaidTuition = GetLastPaidTuition(selectedCourseRegistrationId);

                List<CourseRegistrationDetail> currentCRDList = (await _courseRegistrationDetailService.GetAllCRD())
                .Where(c => c.CourseRegistrationFormId == selectedCourseRegistrationId).ToList();

                List<Subject> currentSubjectList = subjectList
                .Where(item2 => currentCRDList.Exists(item => item.SubjectId == item2.Id)).ToList();

                double totalTuition = 0;
                foreach (var item in currentSubjectList)
                {
                    var currentSubjectType = await _subjectTypeService.GetSubjectTypeById(item.SubjectTypeId);
                    if (currentSubjectType != null)
                    {
                        totalTuition += item.NumberOfCredits * currentSubjectType.LessonsCharge;
                    }
                }
                CurrentTotalTuition = totalTuition;

                PriorityType maxPriorityType = priorityTypeList
                .Where(item => studentPriorityTypeList.Exists(c => c.PriorityTypeId == item.Id))
                .OrderByDescending(item => item.TuitionDiscountRate)
                .FirstOrDefault();
                if (maxPriorityType != null)
                {
                    CurrentRealityTution = CurrentTotalTuition * (1 - maxPriorityType.TuitionDiscountRate);
                    ToolTipRealityTuition = maxPriorityType.TuitionDiscountRate * 100 + "% discount for priority subjects: " + maxPriorityType.PriorityName;
                }

                CurrentPaidTuition = thisTuitionFeeReceiptList.Sum(item => item.Charge);

                CurrentRemainTution = (CurrentRealityTution - CurrentPaidTuition >= 0) ? (CurrentRealityTution - CurrentPaidTuition) : 0;
            }
        }
        #endregion
        #region Helper
        public void ReloadCourseRegistrationDisplays(List<CourseRegistrationForm> courseRegistrationFormList)
        {
            primaryCourseRegistrationDisplayList.Clear();
            CourseRegistrationDisplayList = null;

            if (courseRegistrationFormList.Count > 0)
            {
                primaryCourseRegistrationDisplayList.AddRange(
                from item in courseRegistrationFormList
                join item2 in semesterList on item.SemesterId equals item2.Id
                select new CourseRegistrationDisplay
                {
                    CourseRegistrationRequester = this,
                    CourseRegistrationId = item.Id,
                    CourseRegistrationSemester = GetFormattedSemester(item2.SemesterName),
                    CourseRegistrationSchoolYear = item2.Year.ToString(),
                    CourseRegistrationCreateDate = item.CreatedDate.ToString("dd/MM/yyyy"),
                    LastPaidTuitionDate = GetLastPaidTuition(item.Id)
                });

                CourseRegistrationDisplayList = primaryCourseRegistrationDisplayList
                    .Where(d => !string.IsNullOrEmpty(d.LastPaidTuitionDate))
                    .OrderBy(d => d.CourseRegistrationId).ToObservableCollection();
                ReloadCourseRegistrationItemsBackground();
            }
        }
        private string GetFormattedSemester(SemesterName semesterName)
        {
            return semesterName switch
            {
                SemesterName.FirstSemester => "Học Kỳ I",
                SemesterName.SecondSemester => "Học Kỳ II",
                SemesterName.SummerSemester => "Học Kỳ hè",
                _ => throw new NotImplementedException(),
            };
        }
        private string GetLastPaidTuition(int courseRegistrationFormId)
        {
            List<TuitionFeeReceipt> currentTuitionFeeReceiptList = new List<TuitionFeeReceipt>();
            foreach(var item in tuitionFeeReceiptList)
            {
                if(item.CourseRegistrationFormId== courseRegistrationFormId)
                {
                    currentTuitionFeeReceiptList.Add(item);
                }
            }
            TuitionFeeReceipt maxDateReceipt = currentTuitionFeeReceiptList.OrderByDescending(r => r.CreatedDate).FirstOrDefault();
            if (maxDateReceipt != null)
            {
                return maxDateReceipt.CreatedDate.ToString("dd/MM/yyyy");
            }
            return "";
        }

        public void ReloadCourseRegistrationItemsBackground()
        {
          if (CourseRegistrationDisplayList != null && CourseRegistrationDisplayList.Count > 0)
            {
                for (int i = 0; i < CourseRegistrationDisplayList.Count; i++)
                {
                    CourseRegistrationDisplayList[i].CourseRegistrationBackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
                }
            }
        }
        #endregion
    }
}
