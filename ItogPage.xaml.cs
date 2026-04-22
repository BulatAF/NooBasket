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

    // Ётот метод срабатывает, когда страница по€вл€етс€ на экране
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _results = await _engine.LoadGlobalProgress();
        if (_results != null && _results.ContainsKey(_nameJSON))
        {
            var stats = _results[_nameJSON];

            // ¬ыводим данные (F1 Ч один знак после зап€той)
            Persent.Text = $"{stats.Percent:F1}%";
            NumOfCorrect.Text = $"{stats.NumberOfCorrect} из {stats.NumberOfAll}";
        }
    }
    private async void GoToMenu(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MenuTests");
    }
}
