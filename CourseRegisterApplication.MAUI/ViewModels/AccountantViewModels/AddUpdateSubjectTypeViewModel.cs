using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AddUpdateSubjectTypeViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string commandName = "";

        public int SubjectTypeId { get; set; } = -1;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateSubjectTypeCommand))]
        private string subjectTypeName = "";

        [ObservableProperty] private string subjectTypeNameMessageText;

        [ObservableProperty] private Color subjectTypeNameColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateSubjectTypeCommand))]
        private string numberOfPeriod = "";

        [ObservableProperty] private string numberOfPeriodMessageText;

        [ObservableProperty] private Color numberOfPeriodColor;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateSubjectTypeCommand))]
        private string subjectTypeFee = "";

        [ObservableProperty] private string subjectTypeFeeMessageText;

        [ObservableProperty] private Color subjectTypeFeeColor;

        #endregion

        #region Constructor
        public AddUpdateSubjectTypeViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            ClearState();

            Popup popup = _serviceProvider.GetService<AddUpdateSubjectTypePopup>();
            await popup.CloseAsync();
        }

        [RelayCommand(CanExecute = nameof(CanAddUpdateBranchExecuted))]
        public async Task AddUpdateBranch()
        {
            if (CommandName == "Add subject type")
            {
                await AddBranch();
            }
            else if (CommandName == "Update subject type")
            {
                await UpdateBranch();
            }
        }

        public bool CanAddUpdateBranchExecuted()
        {
            bool isValidSubjectTypeName = true;
            bool isValidNumberOfPeriod = true;

            // Validate Subject Type Name
            if (string.IsNullOrEmpty(SubjectTypeName))
            {
                SubjectTypeNameColor = Color.FromArgb("#BF1D28");
                SubjectTypeNameMessageText = "Subject type name cannot be empty.";
                isValidSubjectTypeName = false;
            }
            else
            {
                SubjectTypeNameColor = Color.FromArgb("#007D3A");
                SubjectTypeNameMessageText = "Valid subject type name.";
            }

            // Validate Number Of Period
            if (string.IsNullOrEmpty(NumberOfPeriod))
            {
                NumberOfPeriodColor = Color.FromArgb("#BF1D28");
                NumberOfPeriodMessageText = "Number of period cannot be empty.";
                isValidNumberOfPeriod = false;
            }
            else
            {
                NumberOfPeriodColor = Color.FromArgb("#007D3A");
                NumberOfPeriodMessageText = "Valid number of period.";
            }

            return isValidSubjectTypeName && isValidNumberOfPeriod;
        }

        #endregion

        #region Property Changed
        async partial void OnCommandNameChanged(string value)
        {
            var subjectTypeService = _serviceProvider.GetService<ISubjectTypeService>();

            // Load data
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Equals("Update subject type"))
                {
                    var subjectType = await subjectTypeService.GetSubjectTypeById(SubjectTypeId);
                    SubjectTypeName = subjectType.Name;
                    SubjectTypeFee = subjectType.LessonsCharge.ToString();
                    NumberOfPeriod = subjectType.NumberOfLessons.ToString();
                }
            }
        }
        #endregion

        #region Helpers
        private void ClearState()
        {
            CommandName = "";
            SubjectTypeName = "";
            SubjectTypeFee = "";
            NumberOfPeriod = "";
        }

        private async Task AddSubjectType()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new subject type?", "Yes", "No");
            if (accept)
            {
                var subjectTypeService = _serviceProvider.GetService<ISubjectTypeService>();
                var subjectTypes = await subjectTypeService.GetAllSubjectType();

                // Check if there is any branch in the database with the same BranchSpecificId
                var sameSpecifcIdBranches = subjectTypes.Where(b => b.BranchSpecificId.ToLower() == BranchSpecificId.ToLower());
                if (sameSpecifcIdBranches.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this branch because there is another branch with the same Id!", "OK");
                    return;
                }

                // Check if there is any branch in the database with the same BranchName
                var sameNameBranches = subjectTypes.Where(b => b.BranchName.ToLower() == BranchName.ToLower());
                if (sameNameBranches.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this branch because there is another branch with the same name!", "OK");
                    return;
                }

                // Add branch
                var branch = await subjectTypeService.AddBranch(new() { BranchSpecificId = BranchSpecificId.Trim(), BranchName = BranchName.Trim(), DepartmentId = SelectedDepartment.Id });
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
