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

    private async void OnGoToTopic1Page(object sender, EventArgs e)
    {

        await Shell.Current.GoToAsync("//Topic1Page");
    }

    private async void OnGoToTopic2Page(object sender, EventArgs e)
    {
        if (TopicAvailable.IsAvailable(2))
        {
            await Shell.Current.GoToAsync("//Topic2Page");
        }
        else
        {
            await DisplayAlert("Тема закрыта",
                "Сначала завершите Тему 1",
                "Понятно");
        }
    }
}