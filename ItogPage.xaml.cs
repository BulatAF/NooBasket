using NooBasket.Models;
using System.Text.Json;
namespace NooBasket;

public partial class ItogPage : ContentPage
{
    string _nameJSON;
    TestEngine _engine;
    Dictionary<string, TestResult> _results;

    public ItogPage(string nameJSON)
    {
        _nameJSON = nameJSON;
        _engine = new TestEngine();
        InitializeComponent();
    }

    // Этот метод срабатывает, когда страница появляется на экране
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _results = await _engine.LoadGlobalProgress();
        var stats = _results[_nameJSON];

        // Выводим данные (F2 — два знака после запятой)
        Persent.Text = $"{stats.Percent:F2}%";
        NumOfCorrect.Text = $"{stats.NumberOfCorrect} из {stats.NumberOfAll}";
    }
    private async void GoToMenu(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MenuTests");
    }
    protected override bool OnBackButtonPressed()
    {
        // Возвращаем true, чтобы просто "проглотить" нажатие и ничего не делать
        return true;
    }
}
