using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }
    }
}