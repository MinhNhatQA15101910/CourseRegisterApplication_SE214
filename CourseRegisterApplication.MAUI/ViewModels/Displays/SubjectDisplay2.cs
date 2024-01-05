using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.Displays
{
    public partial class SubjectDisplay2 : ObservableObject
    {
        #region Properties
        public ISubjectRequester2 SubjectRequester2 { get; set; }

        [ObservableProperty]
        private string subjectID2;

        [ObservableProperty]
        private string subjectName2;

        [ObservableProperty]
        private int numberOfCredits2;

        [ObservableProperty]
        private string semester2;

        [ObservableProperty]
        private string subjectType2;

        [ObservableProperty]
        private Color backgroundColor2;
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseSubject2()
        {
            SubjectRequester2.ReloadItemsBackground2();
            BackgroundColor2 = Color.FromArgb("#B9D8F2");
            SubjectRequester2.ChooseSubject2(this);

        }
        #endregion
    }

    #region IRequesters
    public interface ISubjectRequester2
    {
        void ReloadItemsBackground2();
        void ChooseSubject2(SubjectDisplay2 subjectDisplay2);
    }
    #endregion
}
