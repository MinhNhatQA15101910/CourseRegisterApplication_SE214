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
        #endregion
    }
}
