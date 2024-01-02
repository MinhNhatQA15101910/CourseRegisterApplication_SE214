using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.Displays
{
    public partial class CourseRegistrationDisplay : ObservableObject
    {
        #region Properties
        public ICourseRegistrationRequester CourseRegistrationRequester { get; set; }

        [ObservableProperty]
        private int courseRegistrationId;

        [ObservableProperty]
        private string courseRegistrationSemester;

        [ObservableProperty]
        private string courseRegistrationSchoolYear;

        [ObservableProperty]
        private string courseRegistrationCreateDate;

        [ObservableProperty]
        private string courseRegistrationStatus;

        [ObservableProperty]
        private Color statusTextColor;

        [ObservableProperty]
        private Color courseRegistrationBackgroundColor;
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseCourseRegistration()
        {
            CourseRegistrationRequester.ReloadCourseRegistrationItemsBackground();
            CourseRegistrationBackgroundColor = Color.FromArgb("#B9D8F2");
            CourseRegistrationRequester.ChooseCourseRegistration(this);

        }
        #endregion
    }

    #region IRequesters
    public interface ICourseRegistrationRequester
    {
        void ReloadCourseRegistrationItemsBackground();
        Task ChooseCourseRegistration(CourseRegistrationDisplay courseRegistrationDisplay);
    }
    #endregion
}
