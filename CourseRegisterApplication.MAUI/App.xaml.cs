using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(UpdateCurriculumPage page)
        {
            InitializeComponent();

            MainPage = page;
        }
    }
}