using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AddSubjectViewModel : ObservableObject
    {
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        [ObservableProperty] private string commandName;

        [ObservableProperty] private string subjectSpecificId;

        [ObservableProperty] private Color subjectSpecificIdColor;

        [ObservableProperty] private string subjectSpecificIdMessageText;

        [ObservableProperty] private string subjectName;

        [ObservableProperty] private Color subjectNameColor;

        [ObservableProperty] private string subjectNameMessageText;

        #endregion

        #region Constructor
        public AddSubjectViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task ClosePopup()
        {
            //ClearState();

            Popup popup = _serviceProvider.GetService<AddSubjectPopup>();
            await popup.CloseAsync();
        }
        #endregion

    }
}
