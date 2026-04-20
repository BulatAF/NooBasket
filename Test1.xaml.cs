namespace NooBasket;

public partial class Test1 : ContentPage
{
	TestEngine engine;
    int _numTest = 0;
    string _nameJSON = "";

	public Test1(int numTest = 0, string nameJSON = "")
	{
        _nameJSON = nameJSON;
        _numTest = numTest;
		InitializeComponent();
        LoadQuestion();
    }
    private async void LoadQuestion()
    {
        engine = new TestEngine();
        await engine.LoadFromJson(_nameJSON);
        Text.Text = engine.Questions?[_numTest].Text;

        string?[] var = engine.Questions[_numTest].Various;
        Num1.Text = var[0];
        Num2.Text = var[1];
        Num3.Text = var[2];
    }

    private void QuestionsClick(object sender, EventArgs e)
    {
    }

    private async void NextPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Test1(_numTest+1, _nameJSON));
    }
}