using NooBasket.ViewModels;

namespace NooBasket;

public partial class TestingPage : ContentPage
{
    public TestingPage()
    {
        InitializeComponent();
        BindingContext = new TestingPageViewModel();
    }
}