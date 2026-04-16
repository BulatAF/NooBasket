namespace NooBasket;

public partial class Test1 : ContentPage
{
	TestEngine engine;
    int _numTest = 1;

	public Test1()
	{
		InitializeComponent();
        LoadQuestion();
    }
    private async void LoadQuestion()
    {
        engine = new TestEngine();
        await engine.LoadFromJson("jsconfig1.json");
        Text.Text = engine.PrintQuestion(_numTest);

        string?[] var = engine.PrintVarAnswer(_numTest);
        Num1.Text = var[0];
        Num2.Text = var[1];
        Num3.Text = var[2];
    }

    private void QuestionsClick(object sender, EventArgs e)
    {
    }
}