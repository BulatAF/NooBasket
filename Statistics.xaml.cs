using NooBasket.Models;
using System.Threading.Tasks;
namespace NooBasket;

public partial class Statistics : ContentPage
{
	TestEngine? _engine;
    Dictionary<string, TestResult> _results;
    public Statistics()
	{
		_engine = new TestEngine();
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _results = await _engine.LoadGlobalProgress();
        _engine.Results = _results;

        ChangedText("Topic1.json", Num1, 1);
        ChangedText("Topic2.json", Num2, 2);
        ChangedText("Topic3.json", Num3, 3);
        ChangedText("Topic4.json", Num4, 4);
        ChangedText("Topic5.json", Num5, 5);
        ChangedText("Topic6.json", Num6, 6);
        ChangedText("Topic7.json", Num7, 7);
        ChangedText("Topic8.json", Num8, 8);
        ChangedText("Topic9.json", Num9, 9);
        ChangedText("Topic10.json", Num10, 10);
        ChangedText("Topic11.json", Num11, 11);
        ChangedText("Topic12.json", Num12, 12);
        ChangedText("Topic13.json", Num13, 13);
        ChangedText("Topic14.json", Num14, 14);
        ChangedText("Topic15.json", Num15, 15);
        ChangedText("Topic16.json", Num16, 16);
        ChangedText("Topic17.json", Num17, 17);
        ChangedText("Topic18.json", Num18, 18);
        ChangedText("Topic19.json", Num19, 19);
    }
    private void ChangedText(string nameJSON, Button button, int numTest)
    {
        if (_results.TryGetValue(nameJSON, out TestResult? stats) && stats.NumberOfAll != 0)
            button.Text = $"Тест {numTest}: Пройден на {stats.Percent:F2}%, набрано {stats.NumberOfCorrect} из {stats.NumberOfAll}";
        else
            button.Text = $"Тест {numTest}: Данные о прохождении этого теста отстутствуют";
    }
    private async void GoToBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void Reset(object sender, EventArgs e)
    {
        bool ans = await Shell.Current.DisplayAlert("Внимание!", "Сбросить весь прогресс?", "Да", "Нет");
        if (ans)
        {
            foreach(string item in _results.Keys.ToList())
            {
                // Создаем новый чистый результат для каждой темы
                _engine.Results[item] = new TestResult
                {
                    Answers = [10],
                    NumberOfAll = 0,
                    NumberOfCorrect = 0
                };
            }
            await _engine.SaveGlobalProgress(_engine.Results);
            OnAppearing();
            await Shell.Current.DisplayAlert("Успех", "Сброс прогресса завершён!", "Хорошо");
        }
    }
}