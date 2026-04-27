using NooBasket.ViewModels;

namespace NooBasket;

public partial class EducationPage : ContentPage
{
    public EducationPage()
    {
        InitializeComponent();
        BindingContext = new EducationViewModel();
    }
}