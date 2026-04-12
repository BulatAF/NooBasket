using NooBasket.ViewModels;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
namespace NooBasket;

public partial class Topic2Page : ContentPage
{
	public Topic2Page()
	{
		InitializeComponent();
        ReadTextFromFile();
	}
    private async void OnGoToEducationPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EducationPage");
    }

    private async void ReadTextFromFile()
    {
        using Stream stream = await FileSystem.OpenAppPackageFileAsync("Topic2Text1.txt");
        using StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        string content = await reader.ReadToEndAsync();

        text1.Text = content;
    }
}