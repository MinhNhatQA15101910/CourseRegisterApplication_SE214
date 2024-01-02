using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.ViewModels.Displays;
using CourseRegisterApplication.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class CourseRegisTrationInfoViewModel : ObservableObject, ICourseRegistrationRequester, ISubjectRequester
    {

        #region Services
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubjectService _subjectService;
        private readonly ISubjectTypeService _subjectTypeService;
        private readonly ISemesterService _semesterService;
        private readonly IStudentService _studentService;
        private readonly ICourseRegistrationFormService _courseRegistrationFormService;
        private readonly ICourseRegistrationDetailService _courseRegistrationDetailService;
        #endregion

        #region Constructor
        public CourseRegisTrationInfoViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _subjectService = _serviceProvider.GetRequiredService<ISubjectService>();
            _subjectTypeService = _serviceProvider.GetRequiredService<ISubjectTypeService>();
            _semesterService = _serviceProvider.GetService<ISemesterService>();
            _studentService = _serviceProvider.GetService<IStudentService>();
            _courseRegistrationFormService = _serviceProvider.GetRequiredService<ICourseRegistrationFormService>();
            _courseRegistrationDetailService = _serviceProvider.GetRequiredService<ICourseRegistrationDetailService>();

        }
        #endregion

        #region Properties
        private readonly List<CourseRegistrationDisplay> primaryCourseRegistrationDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<CourseRegistrationDisplay> courseRegistrationDisplayList = new();

        private readonly List<SubjectDisplay> primarySubjectDisplayList = new();

        [ObservableProperty]
        private ObservableCollection<SubjectDisplay> subjectDisplayList = new(); 

        [ObservableProperty]
        private int totalCredits;

        List<Student> studentList = new List<Student>();
        List<Subject> subjectList = new List<Subject>();
        List<Subject> currentSubjectList = new List<Subject>();
        List<SubjectType> subjectTypeList = new List<SubjectType>();
        List<Semester> semesterList = new List<Semester>();
        List<CourseRegistrationForm> courseRegistrationFormList = new List<CourseRegistrationForm>();
        string selectedSubjectId;
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
            if(thisStudent != null)
            {
                courseRegistrationFormList = await _courseRegistrationFormService.GetCourseRegistrationFormByStudentId(thisStudent.Id);
                subjectList = await _subjectService.GetAllSubjects();
                subjectTypeList = await _subjectTypeService.GetAllSubjectType();
                semesterList = await _semesterService.GetAllSemester();
                ReloadCourseRegistrationDisplays(courseRegistrationFormList);
                SubjectDisplayList = null;
            }

        }
        #endregion

        #region Property Changed
        #endregion

        #region Helper
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

        public void ReloadItemsBackground()
        {
            for (int i = 0; i < SubjectDisplayList.Count; i++)
            {
                SubjectDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public async Task ChooseCourseRegistration(CourseRegistrationDisplay courseRegistrationDisplay)
        {
            selectedCourseRegistrationId = courseRegistrationDisplay.CourseRegistrationId;
            await GetSubjectList(selectedCourseRegistrationId);
            ReloadSubjectDisplays(currentSubjectList);
        }

        public void ChooseSubject(SubjectDisplay subjectDisplay)
        {
            selectedSubjectId = subjectDisplay.SubjectID;
        }

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
                    CourseRegistrationStatus = item.State.ToString(),
                    StatusTextColor = GetFormattedColor(item.State)
                });

                CourseRegistrationDisplayList = primaryCourseRegistrationDisplayList.OrderBy(d => d.CourseRegistrationId).ToObservableCollection();
                ReloadCourseRegistrationItemsBackground();
            }
        }

        public async Task GetSubjectList(int courseRegistratonFormId)
        {
            List<CourseRegistrationDetail> courseRegistratonDetailList = await _courseRegistrationDetailService.GetAllCRD();
            List<CourseRegistrationDetail> currentCourseRegistrationDetailList = courseRegistratonDetailList
                .Where(item => item.CourseRegistrationFormId == courseRegistratonFormId)
                .ToList();
            currentSubjectList.Clear();
            currentSubjectList.AddRange(
                from item in currentCourseRegistrationDetailList
                join item2 in subjectList on item.SubjectId equals item2.Id
                select item2);
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
        private Color GetFormattedColor(CourseRegistrationFormState courseRegistrationFormState)
        {
            if(courseRegistrationFormState == CourseRegistrationFormState.Expired)
            {
                return Color.FromArgb("#BF1D28");
            }
            else if (courseRegistrationFormState == CourseRegistrationFormState.Confirmed)
            {
                return Color.FromArgb("#007D3A");
            }
            else
            {
                return Color.FromArgb("#CC8100");
            }
        }

        public void ReloadSubjectDisplays(List<Subject> subjectList)
        {
            primarySubjectDisplayList.Clear();
            SubjectDisplayList = null;

            if (subjectList.Count > 0)
            {
                primarySubjectDisplayList.AddRange(
                from item in subjectList
                join item2 in subjectTypeList on item.SubjectTypeId equals item2.Id
                select new SubjectDisplay
                {
                    SubjectRequester = this,
                    SubjectID = item.SubjectSpecificId,
                    SubjectName = item.Name,
                    NumberOfCredits = item.NumberOfCredits,
                    SubjectType = item2.Name,
                });
                SubjectDisplayList = primarySubjectDisplayList.OrderBy(d => d.SubjectID).ToObservableCollection();
                TotalCredits = SubjectDisplayList.Sum(item => item.NumberOfCredits);
                ReloadItemsBackground();
            }
        }
        #endregion
    }
}
