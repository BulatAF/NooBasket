using NooBasket.ViewModels;

namespace NooBasket;

public partial class TestingTopicPage : ContentPage
{

    public TestingTopicPage()
    {
        InitializeComponent();
        BindingContext = new TestingTopicPageViewModel();
    }
}