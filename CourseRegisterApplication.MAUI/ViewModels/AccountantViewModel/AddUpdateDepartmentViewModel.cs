﻿using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class AddUpdateDepartmentViewModel : ObservableObject
    {
        #region Service
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string commandName;

        public int DepartmentId { get; set; }

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDeparmentCommand))]
        private string departmentSpecificId;

        [ObservableProperty] private string departmentSpecificIdMessageText;

        [ObservableProperty] private Color departmentSpecificIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDeparmentCommand))]
        private string departmentName;

        [ObservableProperty] private string departmentNameMessageText;

        [ObservableProperty] private Color departmentNameColor;
        #endregion

        #region Variables 
        private int globalVariable1 = 0;
        private int globalVariable2 = 0;
        #endregion

        #region Constructor
        public AddUpdateDepartmentViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand(CanExecute = nameof(CanAddUpdateDepartmentExecuted))]
        public async Task AddUpdateDeparment()
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
            int index1 = 0;
            int index2 = 0;

            // Validate Department ID
            if (string.IsNullOrEmpty(DepartmentSpecificId))
            {
                if (globalVariable1 == 0)
                {
                    DepartmentSpecificIdColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    DepartmentSpecificIdColor = Color.FromArgb("#BF1D28");
                }
                DepartmentSpecificIdMessageText = "Department ID cannot be empty.";
                index1++;
            }
            else
            {
                globalVariable1 = 1;
                DepartmentSpecificIdColor = Color.FromArgb("#007D3A");
                DepartmentSpecificIdMessageText = "Valid Department ID.";
                index1 = 0;

            }

            // Validate Department Name
            if (string.IsNullOrEmpty(DepartmentName))
            {
                if (globalVariable2 == 0)
                {
                    DepartmentNameColor = Color.FromArgb("#FFFFFF");
                }
                else
                {
                    DepartmentNameColor = Color.FromArgb("#BF1D28");
                }
                DepartmentNameMessageText = "Department name cannot be empty.";
                index2++;
            }
            else
            {
                globalVariable2 = 1;
                DepartmentNameColor = Color.FromArgb("#007D3A");
                DepartmentNameMessageText = "Valid department name.";
                index2 = 0;
            }

            if (index1 > 0 || index2 > 0)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Helpers
        private void Clear()
        {
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

                    // Reset department list in the DepartmentManagementPage
                    DepartmentManagementViewModel departmentManagementViewModel = _serviceProvider.GetService<DepartmentManagementViewModel>();
                    departmentManagementViewModel.GetDepartmentsCommand.Execute(null);

                    Clear();
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

                    Clear();

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
