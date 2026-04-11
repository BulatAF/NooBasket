using NooBasket.ViewModels;
using System.IO;
using System.Runtime.InteropServices.Marshalling;
namespace NooBasket;

public partial class Topic1Page : ContentPage
{
	public Topic1Page()
	{
		InitializeComponent();
        text1.Text = "Баскетбол - это командная игра, в которой участвуют две команды по пять игроков на площадке.\r\nЦель каждой команды - забросить мяч в корзину соперника и не дать сопернику забросить мяч в свою корзину.\r\nПобеждает команда, набравшая больше очков к концу игрового времени.\r\nИгра проходит под контролем судей.\r\nВсе участники обязаны соблюдать этические нормы и способствовать честному проведению матча.\r\n";

	}
    private async void OnGoToEducationPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EducationPage");
    }

}