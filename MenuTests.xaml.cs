using System.Threading.Tasks;

namespace NooBasket;

public partial class MenuTests : ContentPage
{
	public MenuTests()
	{
		InitializeComponent();
	}

    private async void Test1(object sender, EventArgs e)
    {
		Button button = (Button)sender;

        switch (button.Text)
        {
            case "义爨 1": await Navigation.PushAsync(new Test1(0, "Topic1.json"));
                break;
            case "义爨 2": await Navigation.PushAsync(new Test1(0, "Topic2.json"));
                break;
            case "义爨 3": await Navigation.PushAsync(new Test1(0, "Topic3.json"));
                break;
            case "义爨 4": await Navigation.PushAsync(new Test1(0, "Topic4.json"));
                break;
            case "义爨 5": await Navigation.PushAsync(new Test1(0, "Topic5.json"));
                break;
            case "义爨 6": await Navigation.PushAsync(new Test1(0, "Topic6.json"));
                break;
            case "义爨 7": await Navigation.PushAsync(new Test1(0, "Topic7.json"));
                break;
            case "义爨 8": await Navigation.PushAsync(new Test1(0, "Topic8.json"));
                break;
            case "义爨 9": await Navigation.PushAsync(new Test1(0, "Topic9.json"));
                break;
            case "义爨 10": await Navigation.PushAsync(new Test1(0, "Topic10.json"));
                break;
            case "义爨 11": await Navigation.PushAsync(new Test1(0, "Topic11.json"));
                break;
            case "义爨 12": await Navigation.PushAsync(new Test1(0, "Topic12.json"));
                break;
            case "义爨 13": await Navigation.PushAsync(new Test1(0, "Topic13.json"));
                break;
            case "义爨 14": await Navigation.PushAsync(new Test1(0, "Topic14.json"));
                break;
            case "义爨 15": await Navigation.PushAsync(new Test1(0, "Topic15.json"));
                break;
            case "义爨 16": await Navigation.PushAsync(new Test1(0, "Topic16.json"));
                break;
            case "义爨 17": await Navigation.PushAsync(new Test1(0, "Topic17.json"));
                break;
            case "义爨 18": await Navigation.PushAsync(new Test1(0, "Topic18.json"));
                break;
            case "义爨 19": await Navigation.PushAsync(new Test1(0, "Topic19.json"));
                break;
        }
    }


    private async void GoToBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}