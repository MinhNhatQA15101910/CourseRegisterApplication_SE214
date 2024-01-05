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

        [RelayCommand(CanExecute = nameof(CanAddUpdateSubjectTypeExecuted))]
        public async Task AddUpdateSubjectType()
        {
            if (CommandName == "Add subject type")
            {
                await AddSubjectType();
            }
            else if (CommandName == "Update subject type")
            {
                await UpdateSubjectType();
            }
        }

        public bool CanAddUpdateSubjectTypeExecuted()
        {
            bool isValidSubjectTypeName = true;
            bool isValidNumberOfPeriod = true;
            bool isValidSubjectTypeFee = true;

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
            else if (!int.TryParse(NumberOfPeriod.Trim(), out int numberOfPeriodValue) || numberOfPeriodValue < 0)
            {
                NumberOfPeriodColor = Color.FromArgb("#BF1D28");
                NumberOfPeriodMessageText = "Number of period is invalid.";
                isValidNumberOfPeriod = false;
            }
            else
            {
                NumberOfPeriodColor = Color.FromArgb("#007D3A");
                NumberOfPeriodMessageText = "Valid number of period.";
            }

            // Validate subject type fee
            if (string.IsNullOrEmpty(SubjectTypeFee))
            {
                SubjectTypeFeeColor = Color.FromArgb("#BF1D28");
                SubjectTypeFeeMessageText = "Subject type fee cannot be empty.";
                isValidNumberOfPeriod = false;
            }
            else if (!double.TryParse(SubjectTypeFee.Trim(), out double subjectTypeFeeValue) || subjectTypeFeeValue < 0)
            {
                SubjectTypeFeeColor = Color.FromArgb("#BF1D28");
                SubjectTypeFeeMessageText = "Subject type fee is invalid.";
                isValidNumberOfPeriod = false;
            }
            else
            {
                SubjectTypeFeeColor = Color.FromArgb("#007D3A");
                SubjectTypeFeeMessageText = "Valid subject type fee.";
            }

            return isValidSubjectTypeName && isValidNumberOfPeriod && isValidSubjectTypeFee;
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

                // Check if there is any subject in the database with the same Subject Name
                var sameNameSubjectType = subjectTypes.Where(st => st.Name.ToLower() == SubjectTypeName.Trim().ToLower());
                if (sameNameSubjectType.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this subject type because there is another subject type with the same name!", "OK");
                    return;
                }

                // Add subject type
                var subjectType = await subjectTypeService.CreateSubjectType(new()
                {
                    Name = SubjectTypeName.Trim(),
                    NumberOfLessons = int.Parse(NumberOfPeriod.Trim()),
                    LessonsCharge = double.Parse(SubjectTypeFee.Trim())
                });

                if (subjectType != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Add subject type successfully!", "OK");

                    // Reset subject type list in the SubjectTypeManagementPage
                    SubjectTypeManagementViewModel subjectTypeManagementViewModel = _serviceProvider.GetService<SubjectTypeManagementViewModel>();
                    subjectTypeManagementViewModel.GetAllSubjectTypeCommand.Execute(null);

                    ClearState();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Add subject type failed!", "OK");
                }
            }
        }

        private async Task UpdateSubjectType()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to update this subject type?", "Yes", "No");
            if (accept)
            {
                var subjectTypeService = _serviceProvider.GetService<ISubjectTypeService>();
                var subjectTypes = await subjectTypeService.GetAllSubjectType();

                // Check if there is any subject type in the database with the same Name
                var sameNameSubjectTypes = subjectTypes.Where(st => st.Name.ToLower() == SubjectTypeName.Trim().ToLower() && st.Id != SubjectTypeId);
                if (sameNameSubjectTypes.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this subject type because there is another subject type with the same name!", "OK");
                    return;
                }

                // Update subject type
                var success = await subjectTypeService.UpdateSubjectType(SubjectTypeId, new() 
                { 
                    Id = SubjectTypeId,
                    Name = SubjectTypeName.Trim(),
                    NumberOfLessons = int.Parse(NumberOfPeriod.Trim()),
                    LessonsCharge = double.Parse(SubjectTypeFee.Trim())
                });

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Update subject type successfully!", "OK");

                    // Reset subject type list in the SubjectTypeManagementPage
                    SubjectTypeManagementViewModel subjectTypeManagementViewModel = _serviceProvider.GetService<SubjectTypeManagementViewModel>();
                    subjectTypeManagementViewModel.GetAllSubjectTypeCommand.Execute(null);

                    ClosePopupCommand.Execute(null);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Update subject type failed!", "OK");
                }
            }
        }
        #endregion
    }
}
