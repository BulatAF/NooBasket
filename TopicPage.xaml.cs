using NooBasket.ViewModels;

namespace NooBasket;

public partial class TopicPage : ContentPage
{
    public TopicPage()
    {
        InitializeComponent();
        BindingContext = new TopicPageViewModel();
    }
}