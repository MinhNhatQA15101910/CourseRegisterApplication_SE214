using CourseRegisterApplication_SE214.Views;
using CourseRegisterApplication_SE214.Views.AdminViews;

namespace CourseRegisterApplication_SE214
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AdminFlyoutPage();
        }
    }
}