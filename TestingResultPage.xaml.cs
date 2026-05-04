using NooBasket.ViewModels;

namespace NooBasket;

public partial class TestingResultPage : ContentPage
{
    public TestingResultPage()
    {
        InitializeComponent();
        BindingContext = new TestingResultPageViewModel();
    }
}