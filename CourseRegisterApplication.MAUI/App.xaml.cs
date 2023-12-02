using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(AdminAppShell page)
        {
            InitializeComponent();

            MainPage = page;
        }

    }
}