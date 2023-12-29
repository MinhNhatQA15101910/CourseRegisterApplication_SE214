using CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.ViewModels.StudentViewModels
{
    public partial class CourseRegistrationViewModel : ObservableObject 
	{
        #region Services
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public CourseRegistrationViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion


        [RelayCommand]
        public async Task GetSetup() { }
    }
}
