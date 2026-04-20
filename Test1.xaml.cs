using Microsoft.Maui;

namespace NooBasket;

public partial class Test1 : ContentPage
{
	TestEngine engine;
    int _numTest = 0;
    string _nameJSON = "";
    bool isClicked = false;
    string _correctAnswer;

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
        Text.Text = engine.Questions?[_numTest].Text;

        string?[] var = engine.Questions[_numTest].Various;

        // Случайное распределение ответов (Первый - верный)
        _correctAnswer = var[0];
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

        // Кнопка перехода между сраницами
        if (_numTest + 1 == engine.Questions.Count)
            NextButton.Text = "Выход";
        else
            NextButton.Text = "Следующий";
    }
    private void QuestionsClick(object sender, EventArgs e)
    {
        // Если ни разу не нажимали, сверяем ответ с правильным и меняем цвет 
        if (!isClicked)
        {
            Button button = (Button)sender;

            if (button.Text == _correctAnswer)
            {
                button.BackgroundColor = Colors.Green;
            }
            else
            {
                button.BackgroundColor = Colors.Red;
            }
            AnswerText.Text = engine.Questions[_numTest].AnswerText;
        }
        isClicked = true;
    }

    private async void NextPage(object sender, EventArgs e)
    {
        // Переход к следующей странице при наличии тестового вопроса, иначе выход в меню
        if (_numTest+1 == engine.Questions.Count)
            await Shell.Current.GoToAsync("//MenuTests");
        else 
            await Navigation.PushAsync(new Test1(_numTest + 1, _nameJSON));
    }
}