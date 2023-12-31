using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.Displays
{
    public partial class SubjectDisplay : ObservableObject
    {
        #region Properties
        public ISubjectRequester SubjectRequester { get; set; }

        [ObservableProperty]
        private string subjectID;

        [ObservableProperty]
        private string subjectName;

        [ObservableProperty]
        private int numberOfCredits;

        [ObservableProperty]
        private string semester;

        [ObservableProperty]
        private string subjectType;

        [ObservableProperty]
        private Color backgroundColor;
        #endregion

        #region Command
        [RelayCommand]
        public void ChooseSubject()
        {
            SubjectRequester.ReloadItemsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");
            SubjectRequester.ChooseSubject(this);

        }
        #endregion
    }

    #region IRequesters
    public interface ISubjectRequester
    {
        void ReloadItemsBackground();
        void ChooseSubject(SubjectDisplay subjectDisplay);
    }
    #endregion
}
