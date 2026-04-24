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
		if (button.Text == "义爨 1")
			await Navigation.PushAsync(new Test1(0, "Topic1.json"));
        if (button.Text == "义爨 2")
            await Navigation.PushAsync(new Test1(0, "Topic2.json"));
        if (button.Text == "义爨 3")
            await Navigation.PushAsync(new Test1(0, "Topic3.json"));
        if (button.Text == "义爨 4")
            await Navigation.PushAsync(new Test1(0, "Topic4.json"));
        if (button.Text == "义爨 5")
            await Navigation.PushAsync(new Test1(0, "Topic5.json"));
        if (button.Text == "义爨 6")
            await Navigation.PushAsync(new Test1(0, "Topic6.json"));
        if (button.Text == "义爨 7")
            await Navigation.PushAsync(new Test1(0, "Topic7.json"));
    }
}