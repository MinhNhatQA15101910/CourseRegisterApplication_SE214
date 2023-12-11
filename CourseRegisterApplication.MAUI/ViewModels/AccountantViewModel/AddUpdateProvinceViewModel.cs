using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class AddUpdateProvinceViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public AddUpdateProvinceViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Properties
        [ObservableProperty] private string commandName;

        [ObservableProperty] private int provinceId = -1;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddUpdateProvinceCommand))] 
        private string provinceName;

        [ObservableProperty] private string provinceNameMessageText;

        [ObservableProperty] private Color provinceNameColor;
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            ClearState();

            Popup popup = _serviceProvider.GetService<AddUpdateProvincePopup>();
            await popup.CloseAsync();
        }

        [RelayCommand(CanExecute = nameof(CanAddUpdateProvinceExecuted))]
        public async Task AddUpdateProvince()
        {
            if (CommandName == "Add province")
            {
                await AddProvince();
            }
            else if (CommandName == "Update province")
            {
                await UpdateProvince();
            }
        }

        public bool CanAddUpdateProvinceExecuted()
        {
            bool isValidProvinceName = true;

            // Validate Province Name
            if (string.IsNullOrEmpty(ProvinceName))
            {
                isValidProvinceName = false;
                ProvinceNameColor = Color.FromArgb("#BF1D28");
                ProvinceNameMessageText = "Province Name cannot be empty.";
            }
            else
            {
                ProvinceNameColor = Color.FromArgb("#007D3A");
                ProvinceNameMessageText = "Valid Province Name.";
            }

            return isValidProvinceName;
        }
        #endregion

        #region Helpers
        private void ClearState()
        {
            CommandName = "";
            ProvinceId = -1;
            ProvinceName = "";
        }

        private void ClearData()
        {
            ProvinceId = -1;
            ProvinceName = "";
        }

        private async Task AddProvince()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to add this new province?", "Yes", "No");
            if (accept)
            {
                var provinceService = _serviceProvider.GetService<IProvinceService>();
                var provinces = await provinceService.GetAllProvinces();

                // Check if there is any province in the database with the same ProvinceName
                var sameNameProvinces = provinces.Where(d => d.ProvinceName.ToLower() == ProvinceName.ToLower());
                if (sameNameProvinces.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot add this province because there is another province with the same name!", "OK");
                    return;
                }

                // Add province
                var province = await provinceService.AddProvince(new() { ProvinceName = ProvinceName.Trim() });
                if (province != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Add province successfully!", "OK");

                    // Reset province list in the ProvinceDistrictManagementPage
                    var provinceDistrictManagementViewModel = _serviceProvider.GetService<ProvinceDistrictManagementViewModel>();
                    provinceDistrictManagementViewModel.GetProvincesCommand.Execute(null);

                    provinceDistrictManagementViewModel.SearchProvinceFilter = "";
                    provinceDistrictManagementViewModel.SearchDistrictFilter = "";
                    provinceDistrictManagementViewModel.ReloadDistrictDisplays(new());
                    provinceDistrictManagementViewModel.ClearProperties();

                    ClearData();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Add province failed!", "OK");
                }
            }
        }

        private async Task UpdateProvince()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question", "Do you want to update this province?", "Yes", "No");
            if (accept)
            {
                var provinceService = _serviceProvider.GetService<IProvinceService>();
                var provinces = await provinceService.GetAllProvinces();

                // Check if there is any province in the database with the same ProvinceName with the updated province
                var sameNameProvinces = provinces.Where(d => d.ProvinceName.ToLower() == ProvinceName.ToLower() && d.Id != ProvinceId);
                if (sameNameProvinces.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot update this province because there is another province with the same name!", "OK");
                    return;
                }

                // Update province
                var success = await provinceService.UpdateProvince(ProvinceId, new() { Id = ProvinceId, ProvinceName = ProvinceName.Trim() });
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Update province successfully!", "OK");

                    // Reset province list in the ProvinceDistrictManagementPage
                    var provinceDistrictManagementViewModel = _serviceProvider.GetService<ProvinceDistrictManagementViewModel>();
                    provinceDistrictManagementViewModel.GetProvincesCommand.Execute(null);

                    provinceDistrictManagementViewModel.SearchProvinceFilter = "";
                    provinceDistrictManagementViewModel.SearchDistrictFilter = "";
                    provinceDistrictManagementViewModel.ReloadDistrictDisplays(new());
                    provinceDistrictManagementViewModel.ClearProperties();

                    // Close Popup
                    ClosePopupCommand.Execute(null);

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Update province failed!", "OK");
                }
            }
        }
        #endregion
    }
}
