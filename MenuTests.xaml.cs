using System.Threading.Tasks;

namespace NooBasket;

public partial class MenuTests : ContentPage
{
	public MenuTests()
	{
		InitializeComponent();
	}

    private async void Test(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Test1());
    }
}