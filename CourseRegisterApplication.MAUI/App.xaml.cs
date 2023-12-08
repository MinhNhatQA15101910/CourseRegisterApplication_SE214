using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(BranchManagementPage page)
        {
            InitializeComponent();

            MainPage = page;
        }
    }
}