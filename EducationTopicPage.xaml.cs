using NooBasket.ViewModels;

namespace NooBasket;

public partial class EducationTopicPage : ContentPage
{
    public EducationTopicPage()
    {
        InitializeComponent();
        BindingContext = new EducationTopicPageViewModel();
    }
}