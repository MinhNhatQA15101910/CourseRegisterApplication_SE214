using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.ViewModels.Displays;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.StudentViews;
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

        [ObservableProperty]
        private string chargeNumber;

        List<Student> studentList = new List<Student>();
        List<PriorityType> priorityTypeList = new List<PriorityType>();
        List<StudentPriorityType> studentPriorityTypeList = new List<StudentPriorityType>();
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
        public async Task GetSetUp()
        {
            studentList = await _studentService.GetStudents();
            Student thisStudent = studentList.Find(item => item.StudentSpecificId == GlobalConfig.CurrentUser.Username);
            if (thisStudent != null)
            {
                courseRegistrationFormList = await _courseRegistrationFormService.GetCourseRegistrationFormByStudentId(thisStudent.Id);
                tuitionFeeReceiptList=await _tuitionFeeReceiptService.GetAllTuitionFeeReceipt();
                semesterList = await _semesterService.GetAllSemester();
                priorityTypeList = await _priorityTypeService.GetAllPriorityType();
                studentPriorityTypeList = await _studentPriorityTypeService.GetStudentPriorityTypesByStudentId(thisStudent.Id);
                IsVisibleTuitionInfo = false;

                ReloadCourseRegistrationDisplays(courseRegistrationFormList);
            }
        }

        [RelayCommand]
        public async Task DisplayPaymentPopup()
        {
            var paymentPopup = _serviceProvider.GetService<PaymentPopup>();
            await Application.Current.MainPage.ShowPopupAsync(paymentPopup);
        }

        [RelayCommand]
        public async Task ClosePopup()
        {
            Popup popup = _serviceProvider.GetService<PaymentPopup>();
            await popup.CloseAsync();
        }

        [RelayCommand]
        public async Task PaymentButton()
        
        {
            if (double.TryParse(ChargeNumber.ToString(), out double chargeValue) && chargeValue > 0)
            {
                TuitionFeeReceipt tuitionFeeReceipt = new TuitionFeeReceipt
                {
                    Charge = chargeValue,
                    CreatedDate = DateTime.Now,
                    CourseRegistrationFormId = selectedCourseRegistrationId,
                    State = TuitionFeeReceiptState.Pending
                };
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid charge value format", "Cancel");
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

                foreach (var item in courseRegistrationFormList)
                {
                    if (item.Id == selectedCourseRegistrationId)
                    {
                        CurrentTotalTuition = item.TotalCharge;
                    }
                }
                
                PriorityType maxPriorityType = priorityTypeList
                .Where(item => studentPriorityTypeList.Exists(c => c.PriorityTypeId == item.Id))
                .OrderByDescending(item => item.TuitionDiscountRate)
                .FirstOrDefault();
                if (maxPriorityType != null)
                {
                    CurrentRealityTution = CurrentTotalTuition * (1 - maxPriorityType.TuitionDiscountRate);
                    int roundValue = (int)Math.Round(CurrentRealityTution);
                    int roundPercent = (int)Math.Round(maxPriorityType.TuitionDiscountRate * 100);
                    CurrentRealityTution = roundValue;

                    ToolTipRealityTuition = roundPercent + "% discount for priority subjects: " + maxPriorityType.PriorityName;
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
                //where item.State == CourseRegistrationFormState.Confirmed
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
