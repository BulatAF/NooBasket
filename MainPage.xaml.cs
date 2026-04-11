using NooBasket.ViewModels;

namespace NooBasket
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private async void OnGoToEducationPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//EducationPage");
        }

        private async void OnGoToTestingPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//TestingPage");
        }


    }

}
