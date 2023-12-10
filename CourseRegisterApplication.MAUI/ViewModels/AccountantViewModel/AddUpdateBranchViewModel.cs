using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class AddUpdateBranchViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string commandName = "";

        public int BranchId { get; set; } = -1;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateBranchCommand))]
        private string branchSpecificId = "";

        [ObservableProperty] private string branchSpecificIdMessageText;

        [ObservableProperty] private Color branchSpecificIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateBranchCommand))]
        private string branchName = "";

        [ObservableProperty] private string branchNameMessageText;

        [ObservableProperty] private Color branchNameColor;

        [ObservableProperty] private ObservableCollection<Department> departmentList = null;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateBranchCommand))]
        private Department selectedDepartment = null;

        [ObservableProperty] private string departmentMessageText;

        [ObservableProperty] private Color departmentColor;
        #endregion

        #region Constructor
        public AddUpdateBranchViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            ClearState();

            Popup popup = _serviceProvider.GetService<AddUpdateBranchPopup>();
            await popup.CloseAsync();
        }

        [RelayCommand]
        public async Task GetDepartments()
        {
            var departmentService = _serviceProvider.GetService<IDepartmentService>();
            DepartmentList = (await departmentService.GetAllDepartments()).ToObservableCollection();
        }

        [RelayCommand(CanExecute = nameof(CanAddUpdateBranchExecuted))]
        public async Task AddUpdateBranch()
        {
            if (CommandName == "Add branch")
            {
                await AddBranch();
            }
            else if (CommandName == "Update branch")
            {
                await UpdateBranch();
            }
        }

        public bool CanAddUpdateBranchExecuted()
        {
            bool isValidBranchSpecificId = true;
            bool isValidBranchName = true;
            bool isValidDepartment = true;

            // Validate Branch ID
            if (string.IsNullOrEmpty(BranchSpecificId))
            {
                BranchSpecificIdColor = Color.FromArgb("#BF1D28");
                BranchSpecificIdMessageText = "Branch ID cannot be empty.";
                isValidBranchSpecificId = false;
            }
            else
            {
                BranchSpecificIdColor = Color.FromArgb("#007D3A");
                BranchSpecificIdMessageText = "Valid Branch ID.";
            }

            // Validate Branch Name
            if (string.IsNullOrEmpty(BranchName))
            {
                BranchNameColor = Color.FromArgb("#BF1D28");
                BranchNameMessageText = "Branch name cannot be empty.";
                isValidBranchName = false;
            }
            else
            {
                BranchNameColor = Color.FromArgb("#007D3A");
                BranchNameMessageText = "Valid branch name.";
            }

            // Validate Department
            if (SelectedDepartment == null)
            {
                DepartmentColor = Color.FromArgb("#BF1D28");
                DepartmentMessageText = "You must select a department!";
                isValidBranchName = false;
            }
            else
            {
                DepartmentColor = Color.FromArgb("#007D3A");
                DepartmentMessageText = "Valid Department!";
            }

            return isValidBranchSpecificId && isValidBranchName && isValidDepartment;
        }

        [RelayCommand]
        public async Task DisplayAddDepartmentPopup()
        {
            var addUpdateDepartmentPopup = _serviceProvider.GetService<AddUpdateDepartmentPopup>();
            var addUpdateDepartmentViewModel = _serviceProvider.GetService<AddUpdateDepartmentViewModel>();

            addUpdateDepartmentViewModel.CommandName = "Add department";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateDepartmentPopup);
        }
        #endregion

        #region Property Changed
        async partial void OnCommandNameChanged(string value)
        {
            var branchService = _serviceProvider.GetService<IBranchService>();

            // Load data
            if (!string.IsNullOrEmpty(value))
            {
                // Get Departments
                await GetDepartmentsCommand.ExecuteAsync(null);

                if (value.Equals("Update branch"))
                {
                    var branch = await branchService.GetBranchById(BranchId);
                    BranchSpecificId = branch.BranchSpecificId;
                    BranchName = branch.BranchName;
                    SelectedDepartment = DepartmentList.First(d => d.Id == branch.DepartmentId);
                }
            }
        }
        #endregion

        #region Helpers
        private void ClearState()
        {
            CommandName = "";
            BranchId = -1;
            BranchSpecificId = "";
            BranchName = "";
            SelectedDepartment = null;
            DepartmentList = null;
        }

        private async Task AddBranch()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new branch?", "Yes", "No");
            if (accept)
            {
                var branchService = _serviceProvider.GetService<IBranchService>();
                var branches = await branchService.GetAllBranches();

                // Check if there is any branch in the database with the same BranchSpecificId
                var sameSpecifcIdBranches = branches.Where(b => b.BranchSpecificId.ToLower() == BranchSpecificId.ToLower());
                if (sameSpecifcIdBranches.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this branch because there is another branch with the same Id!", "OK");
                    return;
                }

                // Check if there is any branch in the database with the same BranchName
                var sameNameBranches = branches.Where(b => b.BranchName.ToLower() == BranchName.ToLower());
                if (sameNameBranches.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this branch because there is another branch with the same name!", "OK");
                    return;
                }

                // Add branch
                var branch = await branchService.AddBranch(new() { BranchSpecificId = BranchSpecificId.Trim(), BranchName = BranchName.Trim(), DepartmentId = SelectedDepartment.Id });
                if (branch != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Add branch successfully!", "OK");

                    // Reset branches list in the BranchManagementPage
                    BranchManagementViewModel branchManagementViewModel = _serviceProvider.GetService<BranchManagementViewModel>();
                    branchManagementViewModel.GetBranchesCommand.Execute(null);

                    ClearState();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Add branch failed!", "OK");
                }
            }
        }

        private async Task UpdateBranch()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to update this branch?", "Yes", "No");
            if (accept)
            {
                var branchService = _serviceProvider.GetService<IBranchService>();
                var branches = await branchService.GetAllBranches();

                // Check if there is any branch in the database with the same BranchSpecificId with the updated one.
                var sameSpecifcIdBranches = branches.Where(b => b.BranchSpecificId.ToLower() == BranchSpecificId.ToLower() && b.Id != BranchId);
                if (sameSpecifcIdBranches.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this branch because there is another branch with the same Id!", "OK");
                    return;
                }

                // Check if there is any branch in the database with the same BranchName
                var sameNameBranches = branches.Where(b => b.BranchName.ToLower() == BranchName.ToLower() && b.Id != BranchId);
                if (sameNameBranches.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this branch because there is another branch with the same name!", "OK");
                    return;
                }

                // Update branch
                var success = await branchService.UpdateBranch(BranchId, new() { Id = BranchId, BranchSpecificId = BranchSpecificId.Trim(), BranchName = BranchName.Trim(), DepartmentId = SelectedDepartment.Id });
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Update branch successfully!", "OK");

                    // Reset branches list in the BranchManagementPage
                    BranchManagementViewModel branchManagementViewModel = _serviceProvider.GetService<BranchManagementViewModel>();
                    branchManagementViewModel.GetBranchesCommand.Execute(null);

                    ClosePopupCommand.Execute(null);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Update branch failed!", "OK");
                }
            }
        }
        #endregion
    }
}
