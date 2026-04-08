using Microsoft.Maui.Controls;
using NooBasket.Services;

namespace NooBasket
{
    public partial class LearningPage : ContentPage
    {
        public LearningPage()
        {
            InitializeComponent();
        }

        private async Task RenderTopicsAsync()
        {
            // На всякий случай очищаем список тем, чтобы при повторном заходе на страницу
            // кнопки не дублировались.
            TopicsStack.Children.Clear();

            // Загружаем все темы из одного файла
            var content = await LearningContentStore.LoadAsync();

            // Создаём кнопку на каждую тему
            foreach (var topic in content.Topics)
            {
                var btn = new Button
                {
                    Text = topic.Title,
                    Style = (Style)Resources["TopicButtonStyle"],
                    CommandParameter = topic.Id.ToString()
                };

                // При клике открываем страницу темы
                btn.Clicked += OnTopicClicked;

                TopicsStack.Children.Add(btn);
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

            // Загружаем темы каждый раз при показе страницы.
            // (Так проще и понятнее. Если захотим — позже можно добавить кэш.)
            _ = RenderTopicsAsync();
        }
    }
}