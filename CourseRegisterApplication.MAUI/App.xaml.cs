using CourseRegisterApplication_SE214.Views;

namespace CourseRegisterApplication_SE214
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }
    }
}