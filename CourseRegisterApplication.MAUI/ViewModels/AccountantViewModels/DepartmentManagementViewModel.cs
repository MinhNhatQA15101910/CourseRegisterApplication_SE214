using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class DepartmentDisplay : ObservableObject
    {
        #region Properties
        public IDepartmentRequester DepartmentRequester { get; set; }

        public int DepartmentId { get; set; }

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
        private readonly IDepartmentService _departmentService;
        private readonly IBranchService _branchService;
        #endregion

        #region Properties
        [ObservableProperty] private ObservableCollection<string> filterOptions = new() { "ID", "Name" };

        [ObservableProperty] private string selectedFilterOption = "ID";

        [ObservableProperty] private string searchFilter;

        private readonly List<DepartmentDisplay> primaryDepartmentDisplayList = new();

        [ObservableProperty] private ObservableCollection<DepartmentDisplay> departmentDisplayList = new();

        private int selectedDepartmentId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DisplayUpdateDepartmentPopupCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteDepartmentCommand))]
        private string selectedDepartmentSpecificIdDisplayText;

        [ObservableProperty] private string selectedDepartmentNameDisplayText = "Department:";
        #endregion

        #region Constructor
        public DepartmentManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _departmentService = serviceProvider.GetService<IDepartmentService>();
            _branchService = serviceProvider.GetService<IBranchService>();
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
        public async Task GetDepartments()
        {
            List<Department> departmentList = await _departmentService.GetAllDepartments();

            ReloadDepartmentDisplays(departmentList);

            DisplayDepartmentInformation(new()
            {
                DepartmentSpecificId = "",
                DepartmentName = ""
            });
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateDepartmentExecuted))]
        public async Task DeleteDepartment()
        {
            bool accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to delete this department", "Yes", "No");
            if (accept)
            {
                // If there is any branch which belongs to the deleted department, display not allow alert.
                var branchList = await _branchService.GetBranchesByDepartmentId(selectedDepartmentId);
                if (branchList!.Count > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You cannot delete this department because there is some branches belong to it!", "OK");
                    return;
                }

                // Delete department
                var success = await _departmentService.DeleteDepartment(selectedDepartmentId);
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Delete department successfully!", "OK");
                    GetDepartmentsCommand.Execute(null);
                }
            }
        }

        [RelayCommand]
        public async Task DisplayAddDepartmentPopup()
        {
            Popup popup = _serviceProvider.GetService<AddUpdateDepartmentPopup>();
            var bindingContext = popup.BindingContext as AddUpdateDepartmentViewModel;

            bindingContext.CommandName = "Add department";

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateDepartmentExecuted))]
        public async Task DisplayUpdateDepartmentPopup()
        {
            Popup popup = _serviceProvider.GetService<AddUpdateDepartmentPopup>();
            var bindingContext = popup.BindingContext as AddUpdateDepartmentViewModel;

            bindingContext.DepartmentId = selectedDepartmentId;
            bindingContext.DepartmentSpecificId = SelectedDepartmentSpecificIdDisplayText;
            bindingContext.DepartmentName = SelectedDepartmentNameDisplayText[12..];
            bindingContext.CommandName = "Update department";

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }

        public bool CanDeleteUpdateDepartmentExecuted()
        {
            return !string.IsNullOrEmpty(SelectedDepartmentSpecificIdDisplayText);
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
                foreach (var department in departmentList)
                {
                    primaryDepartmentDisplayList.Add(new DepartmentDisplay
                    {
                        DepartmentRequester = this,
                        DepartmentId = department.Id,
                        DepartmentSpecificId = department.DepartmentSpecificId,
                        DepartmentName = department.DepartmentName,
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
            selectedDepartmentId = departmentDisplay.DepartmentId;
            SelectedDepartmentSpecificIdDisplayText = $"{departmentDisplay.DepartmentSpecificId}";
            SelectedDepartmentNameDisplayText = $"Department: {departmentDisplay.DepartmentName}";
        }
        #endregion
    }
}
