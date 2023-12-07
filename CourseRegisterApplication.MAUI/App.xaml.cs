using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(DepartmentManagementPage page)
        {
            InitializeComponent();

            MainPage = page;
        }

    }
}