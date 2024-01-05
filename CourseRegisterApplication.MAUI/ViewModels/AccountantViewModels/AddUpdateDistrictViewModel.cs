using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AddUpdateDistrictViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public AddUpdateDistrictViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Properties
        [ObservableProperty] private string commandName = "";

        [ObservableProperty] private int districtId = -1;

        public int ProvinceId { get; set; } = -1;

        [ObservableProperty] private string provinceName = "";

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateDistrictCommand))]
        private string districtName = "";

        [ObservableProperty] private string districtNameMessageText = "";

        [ObservableProperty] private Color districtNameColor;

        [ObservableProperty] private bool isPriority = false;
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            ClearState();

            var popup = _serviceProvider.GetService<AddUpdateDistrictPopup>();
            await popup.CloseAsync();
        }

        [RelayCommand(CanExecute = nameof(CanAddUpdateDistrictExecuted))]
        public async Task AddUpdateDistrict()
        {
            if (CommandName == "Add district")
            {
                await AddDistrict();
            }
            else if (CommandName == "Update district")
            {
                await UpdateDistrict();
            }
        }

        public bool CanAddUpdateDistrictExecuted()
        {
            bool isValidDistrictName = true;

            // Validate District Name
            if (string.IsNullOrEmpty(DistrictName))
            {
                isValidDistrictName = false;
                DistrictNameColor = Color.FromArgb("#BF1D28");
                DistrictNameMessageText = "District Name cannot be empty.";
            }
            else
            {
                DistrictNameColor = Color.FromArgb("#007D3A");
                DistrictNameMessageText = "Valid District Name.";
            }

            return isValidDistrictName;
        }
        #endregion

        #region Helpers
        private void ClearState()
        {
            CommandName = "";

            ProvinceId = -1;
            ProvinceName = "";

            DistrictId = -1;
            DistrictName = "";
            IsPriority = false;
        }

        private void ClearData()
        {
            DistrictId = -1;
            DistrictName = "";
            IsPriority = false;
        }

        private async Task AddDistrict()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new district?", "Yes", "No");
            if (accept)
            {
                var districtService = _serviceProvider.GetService<IDistrictService>();
                var districts = await districtService.GetDistrictsByProvinceId(ProvinceId);

                // Check if there is any district in the province with the same DistrictName
                var sameNameDistricts = districts.Where(d => d.DistrictName.ToLower() == DistrictName.Trim().ToLower());
                if (sameNameDistricts.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this district because there is another district in the province with the same name!", "OK");
                    return;
                }

                // Add district
                var district = await districtService.AddDistrict(new() { DistrictName = DistrictName.Trim(), IsPriority = IsPriority, ProvinceId = ProvinceId });
                if (district != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Add district successfully!", "OK");

                    // Reload district list in the ProvinceDistrictManagementPage
                    var provinceDistrictManagementViewModel = _serviceProvider.GetService<ProvinceDistrictManagementViewModel>();

                    var districtList = await districtService.GetDistrictsByProvinceId(ProvinceId);
                    provinceDistrictManagementViewModel.ReloadDistrictDisplays(districtList);

                    provinceDistrictManagementViewModel.ClearDistrictData();

                    ClearData();

                    // Reload district list in AddUpdateStudentPage
                    AddUpdateStudentViewModel addUpdateStudentViewModel = _serviceProvider.GetService<AddUpdateStudentViewModel>();
                    await addUpdateStudentViewModel.ReloadDistrictList();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Add district failed!", "OK");
                }
            }
        }

        private async Task UpdateDistrict()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to update this district?", "Yes", "No");
            if (accept)
            {
                var districtService = _serviceProvider.GetService<IDistrictService>();
                var districts = await districtService.GetDistrictsByProvinceId(ProvinceId);

                // Check if there is any district in the province with the same DistrictName as the updated district
                var sameNameDistricts = districts.Where(d => d.DistrictName.ToLower() == DistrictName.Trim().ToLower() && d.Id != DistrictId);
                if (sameNameDistricts.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this district because there is another district in the province with the same name!", "OK");
                    return;
                }

                // Update district
                var success = await districtService.UpdateDistrict(DistrictId, new() { Id = DistrictId, DistrictName = DistrictName.Trim(), IsPriority = IsPriority, ProvinceId = ProvinceId });
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Update district successfully!", "OK");

                    // Reset district list in the ProvinceDistrictManagementPage
                    var provinceDistrictManagementViewModel = _serviceProvider.GetService<ProvinceDistrictManagementViewModel>();

                    var districtList = await districtService.GetDistrictsByProvinceId(ProvinceId);
                    provinceDistrictManagementViewModel.ReloadDistrictDisplays(districtList);

                    provinceDistrictManagementViewModel.ClearDistrictData();

                    // Close this popup
                    ClosePopupCommand.Execute(null);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Update district failed!", "OK");
                }
            }
        }
        #endregion
    }
}
