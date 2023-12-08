using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class BranchDisplay : ObservableObject
    {
        #region Properties
        public IBranchRequester BranchRequester { get; set; }

        public int BranchId { get; set; }

        [ObservableProperty] private string branchSpecificId;

        [ObservableProperty] private string branchName;

        public int DepartmentId { get; set; }

        [ObservableProperty] private string departmentSpecificId;

        [ObservableProperty] private string departmentName;

        [ObservableProperty] private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public void DisplayBranchInformation()
        {
            BranchRequester.ReloadItemsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            BranchRequester.DisplayBranchInformation(this);
        }
        #endregion
    }

    public interface IBranchRequester
    {
        void ReloadItemsBackground();
        void DisplayBranchInformation(BranchDisplay branchDisplay);
    }

    public partial class BranchManagementViewModel : ObservableObject, IBranchRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        private readonly List<BranchDisplay> primaryBranchDisplayList = new();

        [ObservableProperty] private ObservableCollection<BranchDisplay> branchDisplayList = new();

        [ObservableProperty] private ObservableCollection<string> filterOptions = new() { "ID", "Name", "Department ID" };

        [ObservableProperty] private string selectedFilterOption = "ID";

        [ObservableProperty] private string searchFilter;

        private int selectedBranchId;

        private int selectedDepartmentId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteBranchCommand))]
        [NotifyCanExecuteChangedFor(nameof(DisplayUpdateBranchPopupCommand))]
        private string selectedBranchSpecificIdDisplayText;

        [ObservableProperty]
        private string selectedDepartmentNameDisplayText;

        [ObservableProperty] private string selectedBranchNameDisplayText = "Branch:";
        #endregion

        #region Constructor
        public BranchManagementViewModel(IServiceProvider serviceProvider)
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
        public async Task GetBranches()
        {
            var branchService = _serviceProvider.GetService<IBranchService>();
            var branchList = await branchService.GetAllBranches();

            await ReloadBranchDisplays(branchList);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateBranchExecuted))]
        public async Task DeleteBranch()
        {
            bool accept = await Application.Current.MainPage.DisplayAlert("Warning!", "Do you want to delete this branch", "Yes", "No");
            if (accept)
            {
                // Delete branch
            }
        }

        [RelayCommand]
        public async Task DisplayAddBranchPopup()
        {
            Popup popup = _serviceProvider.GetService<AddUpdateBranchPopup>();
            var bindingContext = popup.BindingContext as AddUpdateBranchViewModel;

            bindingContext.CommandName = "Add branch";

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateBranchExecuted))]
        public async Task DisplayUpdateBranchPopup()
        {
            Popup popup = _serviceProvider.GetService<AddUpdateBranchPopup>();
            var bindingContext = popup.BindingContext as AddUpdateBranchViewModel;

            bindingContext.CommandName = "Update branch";
            bindingContext.BranchId = selectedBranchId;
            bindingContext.BranchSpecificId = SelectedBranchSpecificIdDisplayText;
            bindingContext.BranchName = SelectedBranchNameDisplayText[8..];
            bindingContext.DepartmentId = selectedDepartmentId;
            bindingContext.SelectedDepartment = bindingContext.DepartmentList.First(d => d.Id == bindingContext.DepartmentId);

            await Application.Current.MainPage.ShowPopupAsync(popup);
        }

        public bool CanDeleteUpdateBranchExecuted()
        {
            return !string.IsNullOrEmpty(SelectedBranchSpecificIdDisplayText);
        }
        #endregion

        #region Property Changed
        partial void OnSelectedFilterOptionChanged(string oldValue, string newValue)
        {
            switch (newValue)
            {
                case "ID":
                    BranchDisplayList = primaryBranchDisplayList.OrderBy(b => b.BranchSpecificId).ToObservableCollection();
                    break;
                case "Name":
                    BranchDisplayList = primaryBranchDisplayList.OrderBy(b => b.BranchName).ToObservableCollection();
                    break;
                case "Department ID":
                    BranchDisplayList = primaryBranchDisplayList.OrderBy(b => b.DepartmentId).ToObservableCollection();
                    break;
            }

            SearchFilter = "";
            ReloadItemsBackground();
            DisplayBranchInformation(new()
            {
                BranchSpecificId = "",
                BranchName = "",
                DepartmentName = ""
            });
        }

        partial void OnSearchFilterChanged(string oldValue, string newValue)
        {
            switch (SelectedFilterOption)
            {
                case "ID":
                    BranchDisplayList = primaryBranchDisplayList.Where(b => b.BranchSpecificId.ToLower().Contains(newValue.Trim().ToLower())).OrderBy(b => b.BranchSpecificId).ToObservableCollection();
                    break;
                case "Name":
                    BranchDisplayList = primaryBranchDisplayList.Where(d => d.BranchName.ToLower().Contains(newValue.Trim().ToLower())).OrderBy(d => d.BranchName).ToObservableCollection();
                    break;
                case "Department ID":
                    BranchDisplayList = primaryBranchDisplayList.Where(d => d.DepartmentSpecificId.ToLower().Contains(newValue.Trim().ToLower())).OrderBy(d => d.DepartmentSpecificId).ToObservableCollection();
                    break;
            }

            ReloadItemsBackground();
            DisplayBranchInformation(new()
            {
                BranchSpecificId = "",
                BranchName = "",
                DepartmentName = ""
            });
        }
        #endregion

        #region Helpers
        private async Task ReloadBranchDisplays(List<Branch> branchList)
        {
            primaryBranchDisplayList.Clear();

            if (branchList.Count > 0)
            {
                foreach (var branch in branchList)
                {
                    var departmentService = _serviceProvider.GetService<IDepartmentService>();
                    var department = await departmentService.GetDepartment(branch.DepartmentId);

                    primaryBranchDisplayList.Add(new()
                    {
                        BranchRequester = this,
                        BranchId = branch.Id,
                        BranchSpecificId = branch.BranchSpecificId,
                        BranchName = branch.BranchName,
                        DepartmentId = department.Id,
                        DepartmentSpecificId = department.DepartmentSpecificId,
                        DepartmentName = department.DepartmentName
                    });
                }

                BranchDisplayList = primaryBranchDisplayList.OrderBy(b => b.BranchSpecificId).ToObservableCollection();
                ReloadItemsBackground();
                DisplayBranchInformation(new()
                {
                    BranchSpecificId = "",
                    BranchName = "",
                    DepartmentName = ""
                });
            }
        }

        public void ReloadItemsBackground()
        {
            for (int i = 0; i < BranchDisplayList.Count; i++)
            {
                BranchDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public void DisplayBranchInformation(BranchDisplay branchDisplay)
        {
            selectedBranchId = branchDisplay.BranchId;
            selectedDepartmentId = branchDisplay.DepartmentId;
            SelectedBranchSpecificIdDisplayText = branchDisplay.BranchSpecificId;
            SelectedBranchNameDisplayText = $"Branch: {branchDisplay.BranchName}";
            SelectedDepartmentNameDisplayText = branchDisplay.DepartmentName;
        }
        #endregion
    }
}
