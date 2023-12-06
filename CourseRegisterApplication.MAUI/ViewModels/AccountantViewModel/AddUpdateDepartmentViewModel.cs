namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class AddUpdateDepartmentViewModel : ObservableObject
    {
        #region Properties
        [ObservableProperty] private string commandName;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDeparmentCommand))]
        private string departmentSpecificId;

        [ObservableProperty] private string departmentSpecificIdMessageText;

        [ObservableProperty] private Color departmentSpecificIdColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDeparmentCommand))] 
        private string departmentName;

        [ObservableProperty] private string departmentNameMessageText;

        [ObservableProperty] private Color departmentNameColor;
        #endregion

        #region Commands
        [RelayCommand(CanExecute = nameof(CanAddUpdateDepartmentExecuted))]
        public async Task AddUpdateDeparment()
        {
            if (CommandName == "Add department")
            {

            }
            else if (CommandName == "Update department")
            {

            }
        }

        public bool CanAddUpdateDepartmentExecuted()
        {
            return !string.IsNullOrEmpty(DepartmentSpecificId) && 
                !string.IsNullOrEmpty(DepartmentName);
        }
        #endregion
    }
}
