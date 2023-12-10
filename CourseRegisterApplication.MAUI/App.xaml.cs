using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI
{
    public partial class App : Application
    {
        public App(ProvinceDistrictManagementPage page)
        {
            InitializeComponent();

            MainPage = page;
        }
    }
}