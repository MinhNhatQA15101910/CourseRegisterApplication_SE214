using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AddUpdateDepartmentViewModel : ObservableObject
    {
        #region Service
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string commandName;

        public int DepartmentId { get; set; }

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDepartmentCommand))]
        private string departmentSpecificId;

        [ObservableProperty] private string departmentSpecificIdMessageText;

        [ObservableProperty] private Color departmentSpecificIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDepartmentCommand))]
        private string departmentName;

        [ObservableProperty] private string departmentNameMessageText;

        [ObservableProperty] private Color departmentNameColor;
        #endregion

        #region Constructor
        public AddUpdateDepartmentViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            ClearState();

            Popup popup = _serviceProvider.GetService<AddUpdateDepartmentPopup>();
            await popup.CloseAsync();
        }

        [RelayCommand(CanExecute = nameof(CanAddUpdateDepartmentExecuted))]
        public async Task AddUpdateDepartment()
        {
            if (CommandName == "Add department")
            {
                await AddDepartment();
            }
            else if (CommandName == "Update department")
            {
                await UpdateDepartment();
            }
        }

        public bool CanAddUpdateDepartmentExecuted()
        {
            bool isValidDepartmentSpecificId = true;
            bool isValidDepartmentName = true;

            // Validate Department ID
            if (string.IsNullOrEmpty(DepartmentSpecificId))
            {
                isValidDepartmentSpecificId = false;
                DepartmentSpecificIdColor = Color.FromArgb("#BF1D28");
                DepartmentSpecificIdMessageText = "Department ID cannot be empty.";
            }
            else
            {
                DepartmentSpecificIdColor = Color.FromArgb("#007D3A");
                DepartmentSpecificIdMessageText = "Valid Department ID.";

            }

            // Validate Department Name
            if (string.IsNullOrEmpty(DepartmentName))
            {
                isValidDepartmentName = false;
                DepartmentNameColor = Color.FromArgb("#BF1D28");
                DepartmentNameMessageText = "Department name cannot be empty.";
            }
            else
            {
                DepartmentNameColor = Color.FromArgb("#007D3A");
                DepartmentNameMessageText = "Valid department name.";
            }

            return isValidDepartmentSpecificId && isValidDepartmentName;
        }
        #endregion

        #region Helpers
        private void ClearState()
        {
            DepartmentId = -1;
            DepartmentSpecificId = "";
            DepartmentName = "";
        }

        private async Task AddDepartment()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new department?", "Yes", "No");
            if (accept)
            {
                var departmentService = _serviceProvider.GetService<IDepartmentService>();
                var departments = await departmentService.GetAllDepartments();

                // Check if there is any department in the database with the same DepartmentSpecificId
                var sameSpecifcIdDepartments = departments.Where(d => d.DepartmentSpecificId.ToLower() == DepartmentSpecificId.ToLower());
                if (sameSpecifcIdDepartments.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this department because there is another department with the same Id!", "OK");
                    return;
                }

                // Check if there is any department in the database with the same DepartmentName
                var sameNameDepartments = departments.Where(d => d.DepartmentName.ToLower() == DepartmentName.ToLower());
                if (sameNameDepartments.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this department because there is another department with the same name!", "OK");
                    return;
                }

                // Add department
                var department = await departmentService.AddDepartment(new() { DepartmentSpecificId = DepartmentSpecificId.Trim(), DepartmentName = DepartmentName.Trim() });
                if (department != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Add department successfully!", "OK");

                    // Reload department list in the DepartmentManagementPage
                    DepartmentManagementViewModel departmentManagementViewModel = _serviceProvider.GetService<DepartmentManagementViewModel>();
                    departmentManagementViewModel.GetDepartmentsCommand.Execute(null);

                    // Reload department list in the AddUpdateBranchPopup
                    var addUpdateBranchViewModel = _serviceProvider.GetService<AddUpdateBranchViewModel>();
                    await addUpdateBranchViewModel.GetDepartmentsCommand.ExecuteAsync(null);

                    // Reload department list in AddUpdateStudentPage
                    AddUpdateStudentViewModel addUpdateStudentViewModel = _serviceProvider.GetService<AddUpdateStudentViewModel>();
                    await addUpdateStudentViewModel.ReloadDepartmentList();

                    ClearState();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Add department failed!", "OK");
                }
            }
        }

        private async Task UpdateDepartment()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to update this department?", "Yes", "No");
            if (accept)
            {
                var departmentService = _serviceProvider.GetService<IDepartmentService>();
                var departments = await departmentService.GetAllDepartments();

                // Check if there is any department in the database with the same DepartmentSpecificId with the updated DepartmentSpecificId
                var sameSpecifcIdDepartments = departments.Where(d => d.DepartmentSpecificId.ToLower() == DepartmentSpecificId.ToLower() && d.Id != DepartmentId);
                if (sameSpecifcIdDepartments.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this department because the updated ID already existed in the database!", "OK");
                    return;
                }

                // Check if there is any department in the database with the same DepartmentName
                var sameNameDepartments = departments.Where(d => d.DepartmentName.ToLower() == DepartmentName.ToLower() && d.Id != DepartmentId);
                if (sameNameDepartments.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this department because the updated name already existed in the database!", "OK");
                    return;
                }

                // Add department
                var success = await departmentService.UpdateDepartment(DepartmentId, new() { Id = DepartmentId, DepartmentSpecificId = DepartmentSpecificId.Trim(), DepartmentName = DepartmentName.Trim() });
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Update department successfully!", "OK");

                    // Reset department list in the DepartmentManagementPage
                    DepartmentManagementViewModel departmentManagementViewModel = _serviceProvider.GetService<DepartmentManagementViewModel>();
                    departmentManagementViewModel.GetDepartmentsCommand.Execute(null);

                    ClearState();

                    // Close this popup
                    Popup popup = _serviceProvider.GetService<AddUpdateDepartmentPopup>();
                    await popup.CloseAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Update department failed!", "OK");
                }
            }
        }
        #endregion
    }
}
