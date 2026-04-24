using Microsoft.Maui;
using NooBasket.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace NooBasket;

public partial class Test1 : ContentPage
{
	TestEngine? engine;
    int _numTest = 0;
    string _nameJSON = "";
    bool isClicked = false;
    string? _correctAnswer;

    public Test1(int numTest = 0, string nameJSON = "")
	{
        _nameJSON = nameJSON;
        _numTest = numTest;

        InitializeComponent();
        LoadQuestion();
    }
    private async void LoadQuestion()
    {
        // Чтение JSON файла и запись в engine
        engine = new TestEngine();
        await engine.LoadFromJson(_nameJSON);

        if (engine.Results != null && !engine.Results.ContainsKey(_nameJSON))
        {
            engine.Results[_nameJSON] = new TestResult
            { Answers = new int[engine.Questions!.Count] };
        }
        //if (_numTest == 0 && engine.Results != null && engine.Questions != null)
        //{
        //    // 1. Проверяем, есть ли запись в словаре. Если нет — создаем.
        //    if (!engine.Results.ContainsKey(_nameJSON))
        //    {
        //        engine.Results[_nameJSON] = new TestResult();
        //    }

        //    // 2. Теперь безопасно обнуляем
        //    var current = engine.Results[_nameJSON];
        //    current.Answers = new int[engine.Questions.Count];
        //    current.NumberOfAll = 0;
        //    current.NumberOfCorrect = 0;

        //    // 3. Сохраняем
        //    await engine.SaveGlobalProgress(engine.Results);
        //}

        Text.Text = engine.Questions?[_numTest].Text;
        
        string?[] var = engine.Questions![_numTest].Various;

        // Случайное распределение ответов (Первый - верный)
        _correctAnswer = var[0]!;
        Random random = new Random();

        for (int i = var.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            string? temp = var[i];
            var[i] = var[j];
            var[j] = temp;
        }

        Num1.Text = var[0];
        Num2.Text = var[1];
        Num3.Text = var[2];
        Num4.Text = var[3];

        AnswerText.Text = "Выбирите вариант ответа";

        if (engine.Questions?[_numTest].Image != null)
        {
            QuestionImage.Source = engine.Questions?[_numTest].Image;
            QuestionImage.IsVisible = true;
        }
        else
        {
            QuestionImage.IsVisible = false;
            QuestionImage.Source = null; // Обязательно очищаем
        }

        // Кнопка перехода между сраницами
        if (_numTest + 1 == engine.Questions.Count)
            NextButton.Text = "Выход";
        else
            NextButton.Text = "Следующий";
    }
    private async void QuestionsClick(object sender, EventArgs e)
    {
        // Если ни разу не нажимали, сверяем ответ с правильным и меняем цвет
        if (!isClicked)
        {
            Button button = (Button)sender;
            var currentResults = engine.Results[_nameJSON];

            if (button.Text == _correctAnswer)
            {
                button.BackgroundColor = Colors.Green;
                currentResults.Answers[_numTest] = 1;
                currentResults.NumberOfCorrect += 1;
            }
            else
            {
                button.BackgroundColor = Colors.Red;
                currentResults.Answers[_numTest] = 0;
                // Меняем цвет обводки у правильного ответа
                button.BackgroundColor = Colors.Red;
                MoveBorderColor();
            }
            AnswerText.Text = engine.Questions?[_numTest].AnswerText;
            currentResults.NumberOfAll += 1;

            await engine.SaveGlobalProgress(engine.Results);
        }
        isClicked = true;
    }

    // Функция смены обводки при не правильном ответе
    private void MoveBorderColor()
    {
        if (Num1.Text == _correctAnswer)
            Num1.BorderColor = Colors.Green;
        if (Num2.Text == _correctAnswer)
            Num2.BorderColor = Colors.Green;
        if (Num3.Text == _correctAnswer)
            Num3.BorderColor = Colors.Green;
        if (Num4.Text == _correctAnswer)
            Num4.BorderColor = Colors.Green;
    }

    private async void NextPage(object sender, EventArgs e)
    {
        // Переход к следующей странице при наличии тестового вопроса, иначе выход в меню
        if (_numTest + 1 == engine!.Questions?.Count)
        {
            await Navigation.PushAsync(new ItogPage(_nameJSON));
        }
        else
            await Navigation.PushAsync(new Test1(_numTest + 1, _nameJSON));
    }
}