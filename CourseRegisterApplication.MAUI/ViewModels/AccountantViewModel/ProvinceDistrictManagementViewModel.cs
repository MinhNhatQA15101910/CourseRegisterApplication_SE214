using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModel
{
    public partial class ProvinceDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty] private int provinceCode;

        [ObservableProperty] private string provinceName;

        [ObservableProperty] private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public async Task DisplayDistrictListOfSelectedProvince()
        {

        }
        #endregion
    }

    public partial class DistrictDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty] private int districtCode;

        [ObservableProperty] private string districtName;

        [ObservableProperty] private string provinceName;

        [ObservableProperty] private bool isPriority;

        [ObservableProperty] private Color backgroundColor;
        #endregion

        #region Commands
        [RelayCommand]
        public async Task DisplayDistrictListOfSelectedProvince()
        {

        }
        #endregion
    }

    public partial class ProvinceDistrictManagementViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        private int selectedProvinceId = -1;

        [ObservableProperty] private ObservableCollection<string> provinceFilterOptions = new() { "Code", "Name" };

        [ObservableProperty] private string selectedProvinceFilterOption = "Code";

        [ObservableProperty] private string searchProvinceFilter;

        [ObservableProperty] private ObservableCollection<string> districtFilterOptions = new() { "Code", "Name", "Province" };

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
        public async Task GetProvincesAndDistricts()
        {
            var provinceService = _serviceProvider.GetService<IProvinceService>();

            var provinceList = await provinceService.GetAllProvinces();

            ReloadProvinceDisplays(provinceList);
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
                        ProvinceCode = province.Id,
                        ProvinceName = province.ProvinceName
                    });
                }

                ProvinceDisplayList = primaryProvinceDisplayList.OrderBy(p => p.ProvinceCode).ToObservableCollection();
                ReloadProvincesBackground();
            }
        }

        private void ReloadProvincesBackground()
        {
            for (int i = 0; i < ProvinceDisplayList.Count; i++)
            {
                ProvinceDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }

        private async Task ReloadDistrictDisplays(List<District> districtList)
        {
            primaryDistrictDisplayList.Clear();

            if (districtList.Count > 0)
            {
                foreach (var district in districtList)
                {
                    var provinceService = _serviceProvider.GetService<IProvinceService>();
                    var province = await provinceService.GetProvinceById(district.ProvinceId);

                    primaryDistrictDisplayList.Add(new()
                    {
                        DistrictCode = district.Id,
                        DistrictName = district.DistrictName,
                        ProvinceName = province.ProvinceName,
                        IsPriority = district.IsPriority
                    });
                }
            }
        }

        private void ReloadDistrictsBackground()
        {
            for (int i = 0; i < DistrictDisplayList.Count; i++)
            {
                DistrictDisplayList[i].BackgroundColor = (i % 2 == 0) ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#EBF6FF");
            }
        }
        #endregion
    }
}
