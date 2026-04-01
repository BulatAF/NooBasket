namespace NooBasket
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonClicked(object? sender, EventArgs e)
        {
            DisplayAlert("ПК", "Да, я погорячился", "Дать пк в морду");
        }
    }
}
