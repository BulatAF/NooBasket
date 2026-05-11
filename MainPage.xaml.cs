using NooBasket.Services;
using NooBasket.ViewModels;

namespace NooBasket
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }

}
