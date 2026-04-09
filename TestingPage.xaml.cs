using NooBasket.ViewModels;

namespace NooBasket;

public partial class TestingPage : ContentPage
{
	public TestingPage()
	{
		InitializeComponent();
        BindingContext = new TestingViewModel();
    }

    private async void OnGoToMainPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}