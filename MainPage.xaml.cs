namespace NooBasket;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnLearningClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LearningPage());
    }

    private async void OnTestsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TestsPage());
    }

    private async void OnSupportClicked(object sender, EventArgs e)
    {
        string tgLink = "https://t.me/+YOALAx9A_kNjYzM6";
        try
        {
            await Launcher.OpenAsync(tgLink);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", "Не удалось открыть Telegram номер тел. разработчика: +79922378745", "OK");
        }
    }
}