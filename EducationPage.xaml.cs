using NooBasket.ViewModels;

namespace NooBasket;

public partial class EducationPage : ContentPage
{
	public EducationPage()
	{
		InitializeComponent();
        BindingContext = new EducationViewModel();
    }

    private async void OnGoToMainPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}