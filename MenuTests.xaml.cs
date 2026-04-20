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
		if (button.Text == "Тема 1")
			await Navigation.PushAsync(new Test1(0, "Topic1.json"));
    }
}