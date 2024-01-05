using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;
using CourseRegisterApplication.MAUI.Views.StudentViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(AccountantAppShell page)
        {
            InitializeComponent();

            MainPage = page;
        }
    }
}