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

        TestResult stats1 = _results["Topic1.json"];
        if (stats1.NumberOfAll == 0)
            Num1.Text = "Тест 1: Данные о прохождении этого теста отстутствуют";
        else
            Num1.Text = $"Тест 1: пройден на {stats1.Percent:F2}%, набрано {stats1.NumberOfCorrect} из {stats1.NumberOfAll}";

        TestResult stats2 = _results["Topic2.json"];
        if (stats2.NumberOfAll == 0)
            Num2.Text = "Тест 2: Данные о прохождении этого теста отстутствуют";
        else
            Num2.Text = $"Тест 2: пройден на {stats2.Percent:F2}%, набрано {stats2.NumberOfCorrect} из {stats2.NumberOfAll}";

        TestResult stats3 = _results["Topic3.json"];
        if (stats3.NumberOfAll == 0)
            Num3.Text = "Тест 3: Данные о прохождении этого теста отстутствуют";
        else
            Num3.Text = $"Тест 3: пройден на {stats3.Percent:F2}%, набрано {stats3.NumberOfCorrect} из {stats3.NumberOfAll}";

        TestResult stats4 = _results["Topic4.json"];
        if (stats4.NumberOfAll == 0)
            Num4.Text = "Тест 4: Данные о прохождении этого теста отстутствуют";
        else
            Num4.Text = $"Тест 4: пройден на {stats4.Percent:F2}%, набрано {stats4.NumberOfCorrect} из {stats4.NumberOfAll}";

        TestResult stats5 = _results["Topic5.json"];
        if (stats5.NumberOfAll == 0)
            Num5.Text = "Тест 5: Данные о прохождении этого теста отстутствуют";
        else
            Num5.Text = $"Тест 5: пройден на {stats5.Percent:F2}%, набрано {stats5.NumberOfCorrect} из {stats5.NumberOfAll}";

        TestResult stats6 = _results["Topic6.json"];
        if (stats6.NumberOfAll == 0)
            Num6.Text = "Тест 6: Данные о прохождении этого теста отстутствуют";
        else
            Num6.Text = $"Тест 6: пройден на {stats6.Percent:F2}%, набрано {stats6.NumberOfCorrect} из {stats6.NumberOfAll}";

        TestResult stats7 = _results["Topic7.json"];
        if (stats7.NumberOfAll == 0)
            Num7.Text = "Тест 7: Данные о прохождении этого теста отстутствуют";
        else
            Num7.Text = $"Тест 7: пройден на {stats7.Percent:F2}%, набрано {stats7.NumberOfCorrect} из {stats7.NumberOfAll}";
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