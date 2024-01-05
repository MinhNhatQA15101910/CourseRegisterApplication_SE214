using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(LoginPage page)
        {
            InitializeComponent();
            MainPage = page;
        }
    }
}