using System.Threading.Tasks;
namespace NooBasket
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void NextStr(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MenuTests");
        }
    }
}
