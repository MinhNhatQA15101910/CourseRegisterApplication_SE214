using CommunityToolkit.Maui.Views;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ContentViews;

public partial class ManagerAccountFilterPopup : Popup
{
	public ManagerAccountFilterPopup()
	{
		InitializeComponent();
	}
	void ClosePopup(object sender, TappedEventArgs args)
	{
		Close();
	}
}