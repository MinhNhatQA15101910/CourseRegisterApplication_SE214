using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class DepartmentDisplay : ObservableObject
    {
        #region Properties
        public IDepartmentRequester DepartmentRequester { get; set; }

        [ObservableProperty]
        private string departmentSpecificId;

        [ObservableProperty]
        private string departmentName;

        [ObservableProperty]
        private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public void DisplayDepartmentInformation()
        {
            DepartmentRequester.ReloadItemsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            DepartmentRequester.DisplayDepartmentInformation(this);
        }
        #endregion
    }

    public interface IDepartmentRequester
    {
        void ReloadItemsBackground();
        void DisplayDepartmentInformation(DepartmentDisplay departmentDisplay);
    }

    public partial class DepartmentManagementViewModel : ObservableObject, IDepartmentRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private ObservableCollection<string> filterOptions = new() { "ID", "Name" };

        [ObservableProperty] private string selectedFilterOption = "ID";

        [ObservableProperty] private string searchFilter;

        private readonly List<DepartmentDisplay> primaryDepartmentDisplayList = new();

        [ObservableProperty] private ObservableCollection<DepartmentDisplay> departmentDisplayList = new();

        [ObservableProperty] private string selectedDepartmentSpecificIdDisplayText = "Department ID:";

        [ObservableProperty] private string selectedDepartmentNameDisplayText = "Department:";
        #endregion

        #region Constructor
        public DepartmentManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task Logout()
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (result)
            {
                Application.Current.MainPage = _serviceProvider.GetService<LoginPage>();
            }
        }

        [RelayCommand]
        public void GetDepartments()
        {
            List<Department> departmentList = new()
            {
                new()
                {
                    Id = 1,
                    DepartmentSpecificId = "CNPM",
                    DepartmentName = "Công nghệ phần mềm"
                },
                new()
                {
                    Id = 2,
                    DepartmentSpecificId = "HTTT",
                    DepartmentName = "Hệ thống thông tin"
                },
                new()
                {
                    Id = 3,
                    DepartmentSpecificId = "KHMT",
                    DepartmentName = "Khoa học máy tính"
                },
                new()
                {
                    Id = 4,
                    DepartmentSpecificId = "KTMT",
                    DepartmentName = "Kỹ thuật máy tính"
                },
                new()
                {
                    Id = 5,
                    DepartmentSpecificId = "MMT&TT",
                    DepartmentName = "Mạng máy tính và Truyền thông"
                },
                new()
                {
                    Id = 6,
                    DepartmentSpecificId = "KH&KTTT",
                    DepartmentName = "Khoa học và Kỹ thuật thông tin"
                },
            };

            ReloadDepartmentDisplays(departmentList);
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            switch (newValue)
            {
                case "ID":
                    DepartmentDisplayList = primaryDepartmentDisplayList.OrderBy(d => d.DepartmentSpecificId).ToObservableCollection();
                    break;
                case "Name":
                    DepartmentDisplayList = primaryDepartmentDisplayList.OrderBy(d => d.DepartmentName).ToObservableCollection();
                    break;
            }

            SearchFilter = "";
            ReloadItemsBackground();
            DisplayDepartmentInformation(new()
            {
                DepartmentSpecificId = "",
                DepartmentName = ""
            });
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            switch (SelectedFilterOption)
            {
                case "ID":
                    DepartmentDisplayList = primaryDepartmentDisplayList.Where(d => d.DepartmentSpecificId.Contains(newValue.Trim())).OrderBy(d => d.DepartmentSpecificId).ToObservableCollection();
                    break;
                case "Name":
                    DepartmentDisplayList = primaryDepartmentDisplayList.Where(d => d.DepartmentName.Contains(newValue.Trim())).OrderBy(d => d.DepartmentName).ToObservableCollection();
                    break;
            }

            ReloadItemsBackground();
            DisplayDepartmentInformation(new()
            {
                DepartmentSpecificId = "",
                DepartmentName = ""
            });
        }
        #endregion

        #region Helpers
        private void ReloadDepartmentDisplays(List<Department> departmentList)
        {
            primaryDepartmentDisplayList.Clear();

            if (departmentList.Count > 0)
            {
                for (int i = 0; i < departmentList.Count; i++)
                {
                    primaryDepartmentDisplayList.Add(new DepartmentDisplay
                    {
                        DepartmentRequester = this,
                        DepartmentSpecificId = departmentList[i].DepartmentSpecificId,
                        DepartmentName = departmentList[i].DepartmentName,
                    });
                }

                DepartmentDisplayList = primaryDepartmentDisplayList.OrderBy(d => d.DepartmentSpecificId).ToObservableCollection();
                ReloadItemsBackground();
            }
        }

        public void ReloadItemsBackground()
        {
            for (int i = 0; i < DepartmentDisplayList.Count; i++)
            {
                DepartmentDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayDepartmentInformation(DepartmentDisplay departmentDisplay)
        {
            SelectedDepartmentSpecificIdDisplayText = $"Department ID: {departmentDisplay.DepartmentSpecificId}";
            SelectedDepartmentNameDisplayText = $"Department: {departmentDisplay.DepartmentName}";
        }
        #endregion
    }
}
