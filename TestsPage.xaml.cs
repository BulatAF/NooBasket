using Microsoft.Maui.Controls;

namespace NooBasket
{
    public partial class TestsPage : ContentPage
    {
        private bool[] completedTests = new bool[6];
        private bool[] unlockedTests = new bool[6];

        public TestsPage()
        {
            InitializeComponent();
            LoadProgress();
            UpdateTestsStatus();
        }

        private void LoadProgress()
        {
            // Тест 1 всегда разблокирован
            unlockedTests[0] = true;

            // Загружаем статус пройденных тестов
            for (int i = 0; i < completedTests.Length; i++)
            {
                completedTests[i] = Preferences.Get($"test_{i + 1}_completed", false);

                // Если тест пройден, он автоматически разблокирован
                if (completedTests[i])
                {
                    unlockedTests[i] = true;
                }
            }

            // Загружаем статус разблокировки остальных тестов
            for (int i = 1; i < unlockedTests.Length; i++)
            {
                if (!completedTests[i]) // Если не пройден, проверяем разблокирован ли
                {
                    unlockedTests[i] = Preferences.Get($"test_{i + 1}_unlocked", false);
                }
            }
        }

        private void SaveProgress()
        {
            for (int i = 0; i < completedTests.Length; i++)
            {
                Preferences.Set($"test_{i + 1}_completed", completedTests[i]);
            }

            for (int i = 1; i < unlockedTests.Length; i++)
            {
                Preferences.Set($"test_{i + 1}_unlocked", unlockedTests[i]);
            }
        }

        private void UpdateTestsStatus()
        {
            // Массив кнопок
            Button[] buttons = { Test1Button, Test2Button, Test3Button,
                                 Test4Button, Test5Button, Test6Button };

            string[] testNames = {
                "Тест 1: Основные правила баскетбола",
                "Тест 2: Нарушения и фолы",
                "Тест 3: Броски и очки",
                "Тест 4: Позиции игроков",
                "Тест 5: Тактика и стратегия",
                "Тест 6: Соревнования и турниры"
            };

            int completedCount = 0;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    if (completedTests[i])
                    {
                        // Пройденный тест
                        buttons[i].Style = (Style)Resources["CompletedTestButtonStyle"];
                        buttons[i].IsEnabled = true;
                        buttons[i].Text = $"✓ {testNames[i]}";
                        completedCount++;
                    }
                    else if (unlockedTests[i])
                    {
                        // Доступный, но не пройденный тест
                        buttons[i].Style = (Style)Resources["TestButtonStyle"];
                        buttons[i].IsEnabled = true;
                        buttons[i].Text = testNames[i];
                    }
                    else
                    {
                        // Заблокированный тест
                        buttons[i].Style = (Style)Resources["LockedTestButtonStyle"];
                        buttons[i].Text = $"Тест {i + 1} (заблокировано)";
                    }
                }
            }

            // Обновляем прогресс
            if (ProgressLabel != null)
            {
                ProgressLabel.Text = $"Пройдено тестов: {completedCount} из 6";
            }
        }

        public void CompleteTest(int testNumber)
        {
            // testNumber с 1 до 6
            if (testNumber >= 1 && testNumber <= 6)
            {
                int index = testNumber - 1;

                if (!completedTests[index])
                {
                    completedTests[index] = true;
                    unlockedTests[index] = true;

                    // Разблокируем следующий тест (если есть)
                    if (testNumber < 6)
                    {
                        unlockedTests[testNumber] = true;
                    }

                    SaveProgress();
                    UpdateTestsStatus();

                    DisplayAlert("Отлично!", $"Тест {testNumber} пройден успешно!", "OK");
                }
            }
        }

        private async void OnTestClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string param = button?.CommandParameter?.ToString();

            if (int.TryParse(param, out int testNumber))
            {
                int index = testNumber - 1;

                // Проверяем доступен ли тест
                if (unlockedTests[index] || completedTests[index])
                {
                    // Переход на страницу вопросов теста
                    // await Navigation.PushAsync(new TestQuestionsPage(testNumber, this));

                    // Пока просто демонстрация
                    bool passed = await DisplayAlert("Тест",
                        $"Начать тест {testNumber}? Вопросов: 10",
                        "Начать", "Отмена");

                    if (passed)
                    {
                        // Здесь будет логика прохождения теста
                        // После успешного прохождения вызвать CompleteTest(testNumber)
                        await Task.Delay(100); // Имитация прохождения
                        CompleteTest(testNumber);
                    }
                }
                else
                {
                    await DisplayAlert("Доступ закрыт",
                        $"Сначала пройдите тест {testNumber - 1}", "OK");
                }
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadProgress();
            UpdateTestsStatus();
        }
    }
}