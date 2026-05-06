using NooBasket.ViewModels;

namespace NooBasket;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
        BindingContext = new StatisticsPageViewModel();
    }

    //чтобы статистика обновлялась не выходя из приложения
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StatisticsPageViewModel viewModel)
        {
            viewModel.LoadStatistics();
        }
    }
}