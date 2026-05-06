using NooBasket.ViewModels;

namespace NooBasket
{
    public MainPage()
    {
        public MainPage()
        await Navigation.PushAsync(new TestsPage());
    }

    private async void OnSupportClicked(object sender, EventArgs e)
    {
        string tgLink = "https://t.me/+YOALAx9A_kNjYzM6";
        try
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }      
    }

}
