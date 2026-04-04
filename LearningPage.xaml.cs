using Microsoft.Maui.Controls;

namespace NooBasket
{
    public partial class LearningPage : ContentPage
    {
        private bool[] unlockedTopics = new bool[6];

        public LearningPage()
        {
            InitializeComponent();
            LoadProgress();
            UpdateTopicsStatus();
        }

        private void LoadProgress()
        {
            // Тема 1 всегда разблокирована
            unlockedTopics[0] = true;

            // Загружаем статус остальных тем
            for (int i = 1; i < unlockedTopics.Length; i++)
            {
                unlockedTopics[i] = Preferences.Get($"topic_{i + 1}_unlocked", false);
            }
        }

        private void SaveProgress()
        {
            for (int i = 1; i < unlockedTopics.Length; i++)
            {
                Preferences.Set($"topic_{i + 1}_unlocked", unlockedTopics[i]);
            }
        }

        private void UpdateTopicsStatus()
        {
            // Массив кнопок (имена должны совпадать с x:Name в XAML)
            Button[] buttons = { Topic1Button, Topic2Button, Topic3Button,
                                 Topic4Button, Topic5Button, Topic6Button };

            string[] topicNames = {
                "Тема 1: Основные правила баскетбола",
                "Тема 2: Нарушения и фолы",
                "Тема 3: Броски и очки",
                "Тема 4: Позиции игроков",
                "Тема 5: Тактика и стратегия",
                "Тема 6: Соревнования и турниры"
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    if (unlockedTopics[i])
                    {
                        buttons[i].Style = (Style)Resources["TopicButtonStyle"];
                        buttons[i].IsEnabled = true;
                        buttons[i].Text = topicNames[i];
                    }
                    else
                    {
                        buttons[i].Style = (Style)Resources["LockedTopicButtonStyle"];
                        buttons[i].Text = $"Тема {i + 1} (заблокировано)";
                    }
                }
            }
        }

        public void UnlockNextTopic(int completedTopic)
        {
            if (completedTopic >= 1 && completedTopic < 6)
            {
                unlockedTopics[completedTopic] = true;
                SaveProgress();
                UpdateTopicsStatus();
                DisplayAlert("Успех", $"Тема {completedTopic + 1} разблокирована", "OK");
            }
        }

        private async void OnTopicClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string param = button?.CommandParameter?.ToString();

            if (int.TryParse(param, out int topicNumber))
            {
                await Navigation.PushAsync(new TopicDetailPage(topicNumber));
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
            UpdateTopicsStatus();
        }
    }
}