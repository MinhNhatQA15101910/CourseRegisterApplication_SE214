using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class ProvinceDisplay : ObservableObject
    {
        #region Properties
        public IProvinceRequester ProvinceRequester { get; set; }

        [ObservableProperty] private int provinceCode;

        [ObservableProperty] private string provinceName;

        [ObservableProperty] private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public void DisplayDistrictListOfSelectedProvince()
        {
            ProvinceRequester.ReloadProvincesBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            ProvinceRequester.DisplayDistrictList(this);
        }
        #endregion
    }

    public partial class DistrictDisplay : ObservableObject
    {
        #region Properties
        public IDistrictRequester DistrictRequester {  get; set; }

        [ObservableProperty] private int districtCode;

        [ObservableProperty] private string districtName;

        [ObservableProperty] private string provinceName;

        [ObservableProperty] private bool isPriority;

        [ObservableProperty] private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public void GetSelectedDistrictProperties()
        {
            DistrictRequester.ReloadDistrictsBackground();
            BackgroundColor = Color.FromArgb("#B9D8F2");

            DistrictRequester.GetSelectedDistrictProperties(this);
        }
        #endregion
    }

    public interface IProvinceRequester
    {
        void DisplayDistrictList(ProvinceDisplay provinceDisplay);
        void ReloadProvincesBackground();
    }

    public interface IDistrictRequester
    {
        void GetSelectedDistrictProperties(DistrictDisplay districtDisplay);
        void ReloadDistrictsBackground();
    }

    public partial class ProvinceDistrictManagementViewModel : ObservableObject, IProvinceRequester, IDistrictRequester
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DisplayUpdateProvincePopupCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteProvinceCommand))]
        [NotifyCanExecuteChangedFor(nameof(DisplayAddDistrictPopupCommand))]
        private int selectedProvinceId = -1;

        private string selectedProvinceName = "";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DisplayUpdateDistrictPopupCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteDistrictCommand))]
        private int selectedDistrictId = -1;

        private string selectedDistrictName = "";

        private bool selectedDistrictPriority = false;

        [ObservableProperty] private ObservableCollection<string> provinceFilterOptions = new() { "Code", "Name" };

        [ObservableProperty] private string selectedProvinceFilterOption = "Code";

        [ObservableProperty] private string searchProvinceFilter;

        [ObservableProperty] private ObservableCollection<string> districtFilterOptions = new() { "Code", "Name" };

        [ObservableProperty] private string selectedDistrictFilterOption = "Code";

        [ObservableProperty] private string searchDistrictFilter;

        private readonly List<ProvinceDisplay> primaryProvinceDisplayList = new();

        [ObservableProperty] private ObservableCollection<ProvinceDisplay> provinceDisplayList = new();

        private readonly List<DistrictDisplay> primaryDistrictDisplayList = new();

        [ObservableProperty] private ObservableCollection<DistrictDisplay> districtDisplayList = new();
        #endregion

        #region Constructor
        public ProvinceDistrictManagementViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task Logout()
        {
            var accept = await Application.Current.MainPage.DisplayAlert("Question?", "Do you want to logout?", "Yes", "No");
            if (accept)
            {
                Application.Current.MainPage = _serviceProvider.GetService<LoginPage>();
            }
        }

        [RelayCommand]
        public async Task GetProvinces()
        {
            var provinceService = _serviceProvider.GetService<IProvinceService>();

            var provinceList = await provinceService.GetAllProvinces();

            ReloadProvinceDisplays(provinceList);
        }

        [RelayCommand]
        public async Task DisplayAddProvincePopup()
        {
            var addUpdateProvincePopup = _serviceProvider.GetService<AddUpdateProvincePopup>();
            var addUpdateProvinceViewModel = _serviceProvider.GetService<AddUpdateProvinceViewModel>();

            addUpdateProvinceViewModel.CommandName = "Add province";

            await Application.Current.MainPage.ShowPopupAsync(addUpdateProvincePopup);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateProvinceExecuted))]
        public async Task DisplayUpdateProvincePopup()
        {
            var addUpdateProvincePopup = _serviceProvider.GetService<AddUpdateProvincePopup>();
            var addUpdateProvinceViewModel = _serviceProvider.GetService<AddUpdateProvinceViewModel>();

            addUpdateProvinceViewModel.CommandName = "Update province";
            addUpdateProvinceViewModel.ProvinceId = SelectedProvinceId;
            addUpdateProvinceViewModel.ProvinceName = selectedProvinceName;

            await Application.Current.MainPage.ShowPopupAsync(addUpdateProvincePopup);
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateProvinceExecuted))]
        public async Task DeleteProvince()
        {

        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateProvinceExecuted))]
        public async Task DisplayAddDistrictPopup()
        {

        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateDistrictExecuted))]
        public async Task DisplayUpdateDistrictPopup()
        {

        }

        [RelayCommand(CanExecute = nameof(CanDeleteUpdateDistrictExecuted))]
        public async Task DeleteDistrict()
        {

        }

        public bool CanDeleteUpdateProvinceExecuted()
        {
            return SelectedProvinceId != -1;
        }

        public bool CanDeleteUpdateDistrictExecuted()
        {
            return SelectedDistrictId != -1;
        }
        #endregion

        #region Property Changed
        partial void OnSelectedProvinceFilterOptionChanged(string value)
        {
            switch (value)
            {
                case "Code":
                    ProvinceDisplayList = primaryProvinceDisplayList.OrderBy(p => p.ProvinceCode).ToObservableCollection();
                    break;
                case "Name":
                    ProvinceDisplayList = primaryProvinceDisplayList.OrderBy(d => d.ProvinceName).ToObservableCollection();
                    break;
            }

            SearchProvinceFilter = "";
            SearchDistrictFilter = "";
            ReloadProvincesBackground();
            ReloadDistrictDisplays(new());

            ClearProperties();
        }

        partial void OnSearchProvinceFilterChanged(string value)
        {
            switch (SelectedProvinceFilterOption)
            {
                case "Code":
                    ProvinceDisplayList = primaryProvinceDisplayList.Where(p => p.ProvinceCode.ToString().Contains(value.Trim().ToLower())).OrderBy(p => p.ProvinceCode).ToObservableCollection();
                    break;
                case "Name":
                    ProvinceDisplayList = primaryProvinceDisplayList.Where(p => p.ProvinceName.ToLower().Contains(value.Trim().ToLower())).OrderBy(p => p.ProvinceName).ToObservableCollection();
                    break;
            }

            SearchDistrictFilter = "";
            ReloadProvincesBackground();
            ReloadDistrictDisplays(new());

            ClearProperties();
        }

        partial void OnSelectedDistrictFilterOptionChanged(string value)
        {
            switch (value)
            {
                case "Code":
                    DistrictDisplayList = primaryDistrictDisplayList.OrderBy(d => d.DistrictCode).ToObservableCollection();
                    break;
                case "Name":
                    DistrictDisplayList = primaryDistrictDisplayList.OrderBy(d => d.DistrictName).ToObservableCollection();
                    break;
            }

            SearchDistrictFilter = "";
            ReloadDistrictsBackground();
            SelectedDistrictId = -1;
            selectedDistrictName = "";
            selectedDistrictPriority = false;
        }

        partial void OnSearchDistrictFilterChanged(string value)
        {
            switch (SelectedDistrictFilterOption)
            {
                case "Code":
                    DistrictDisplayList = primaryDistrictDisplayList.Where(d => d.DistrictCode.ToString().Contains(value.Trim().ToLower())).OrderBy(p => p.DistrictCode).ToObservableCollection();
                    break;
                case "Name":
                    DistrictDisplayList = primaryDistrictDisplayList.Where(p => p.DistrictName.ToLower().Contains(value.Trim().ToLower())).OrderBy(p => p.DistrictName).ToObservableCollection();
                    break;
            }

            ReloadDistrictsBackground();
            SelectedDistrictId = -1;
            selectedDistrictName = "";
            selectedDistrictPriority = false;
        }
        #endregion

        #region Helpers
        private void ReloadProvinceDisplays(List<Province> provinceList)
        {
            primaryProvinceDisplayList.Clear();

            if (provinceList.Count > 0)
            {
                foreach (var province in provinceList)
                {
                    primaryProvinceDisplayList.Add(new()
                    {
                        ProvinceRequester = this,
                        ProvinceCode = province.Id,
                        ProvinceName = province.ProvinceName
                    });
                }

                ProvinceDisplayList = primaryProvinceDisplayList.OrderBy(p => p.ProvinceCode).ToObservableCollection();
                ReloadProvincesBackground();
            }
        }

        public void ReloadProvincesBackground()
        {
            for (int i = 0; i < ProvinceDisplayList.Count; i++)
            {
                ProvinceDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        private void ReloadDistrictDisplays(List<District> districtList)
        {
            primaryDistrictDisplayList.Clear();

            foreach (var district in districtList)
            {
                primaryDistrictDisplayList.Add(new()
                {
                    DistrictRequester = this,
                    DistrictCode = district.Id,
                    DistrictName = district.DistrictName,
                    ProvinceName = selectedProvinceName,
                    IsPriority = district.IsPriority
                });
            }

            DistrictDisplayList = primaryDistrictDisplayList.OrderBy(d => d.DistrictCode).ToObservableCollection();
            ReloadDistrictsBackground();
        }

        public void ReloadDistrictsBackground()
        {
            for (int i = 0; i < DistrictDisplayList.Count; i++)
            {
                DistrictDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        public async void DisplayDistrictList(ProvinceDisplay provinceDisplay)
        {
            SelectedProvinceId = provinceDisplay.ProvinceCode;
            selectedProvinceName = provinceDisplay.ProvinceName;

            var districtService = _serviceProvider.GetService<IDistrictService>();
            var districtList = await districtService.GetDistrictsByProvinceId(SelectedProvinceId);
            ReloadDistrictDisplays(districtList);

            SelectedDistrictId = -1;
            selectedDistrictName = "";
            selectedDistrictPriority = false;
        }

        public void GetSelectedDistrictProperties(DistrictDisplay districtDisplay)
        {
            SelectedDistrictId = districtDisplay.DistrictCode;
            selectedDistrictName = districtDisplay.DistrictName;
            selectedDistrictPriority = districtDisplay.IsPriority;
        }

        private void ClearProperties()
        {
            SelectedProvinceId = -1;
            selectedProvinceName = "";
            SelectedDistrictId = -1;
            selectedDistrictName = "";
            selectedDistrictPriority = false;
        }
        #endregion
    }
}
