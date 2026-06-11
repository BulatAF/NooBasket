using NooBasket.ViewModels;

namespace NooBasket;

public partial class TestingPage : ContentPage
{
    public TestingPage()
    {
        InitializeComponent();
        BindingContext = new TestingPageViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TestingPageViewModel viewModel)
        {
            viewModel.ChangeButtonColors();
        }
    }
}