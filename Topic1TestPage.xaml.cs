using NooBasket.ViewModels;
using System.IO;
using Microsoft.Maui.Storage;
namespace NooBasket;


public partial class Topic1TestPage : ContentPage
{
    public Topic1TestPage()
    {
        InitializeComponent();
    }
    private async void OnGoToTopic1Page(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Topic1Page");
    }

    private async void OnGoToTopic2Page(object sender, EventArgs e)
    {
        TopicAvailable.CompleteTopic(1);
        await Shell.Current.GoToAsync("//Topic2Page");
    }
}