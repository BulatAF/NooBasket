using Microsoft.Maui.Controls;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket
{
    public partial class TopicDetailPage : ContentPage
    {
        private int _topicNumber;

        public TopicDetailPage(int topicNumber)
        {
            InitializeComponent();   

            // Запоминаем, какую тему нужно показать
            _topicNumber = topicNumber;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // При каждом показе страницы загружаем контент заново
            await LoadTopicContent();
        }

        private async Task LoadTopicContent()
        {
            try
            {
                // 1) Загружаем общий файл со всеми темами
                var content = await LearningContentStore.LoadAsync();

                // 2) Ищем нужную тему по Id
                TopicData? topicData = null;
                foreach (var t in content.Topics)
                {
                    if (t.Id == _topicNumber)
                    {
                        topicData = t;
                        break;
                    }
                }

                // 3) Перед отрисовкой очищаем старый контент
                ContentStack.Children.Clear();

                if (topicData == null)
                {
                    TopicTitleLabel.Text = "Тема не найдена";
                    ContentStack.Children.Add(new Label
                    {
                        Text = $"Не удалось найти тему с id = {_topicNumber}.",
                        TextColor = Colors.Black,
                        FontSize = 18
                    });
                    return;
                }

                // 4) Заголовок темы
                TopicTitleLabel.Text = topicData.Title;

                // 5) Рисуем блоки по очереди (текст/картинка)
                foreach (var block in topicData.Blocks)
                {
                    if (string.Equals(block.Type, "text", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrWhiteSpace(block.Text))
                        {
                            ContentStack.Children.Add(new Label
                            {
                                Text = block.Text,
                                TextColor = Colors.Black,
                                FontSize = 18,
                                LineBreakMode = LineBreakMode.WordWrap
                            });
                        }
                    }
                    else if (string.Equals(block.Type, "image", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrWhiteSpace(block.Image))
                        {
                            ContentStack.Children.Add(new Image
                            {
                                // Картинка хранится в приложении (Resources/Images)
                                Source = ImageSource.FromFile(block.Image),
                                Aspect = Aspect.AspectFit,
                                HeightRequest = 220
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(block.Caption))
                        {
                            ContentStack.Children.Add(new Label
                            {
                                Text = block.Caption,
                                TextColor = Colors.Black,
                                FontSize = 14,
                                HorizontalTextAlignment = TextAlignment.Center,
                                LineBreakMode = LineBreakMode.WordWrap
                            });
                        }
                    }
                }

                // 6) Кнопка теста в конце (пока просто заглушка)
                var testButton = new Button
                {
                    Text = "Пройти тест по теме",
                    BackgroundColor = Color.FromArgb("#405CA3"),
                    TextColor = Color.FromArgb("#FFD66C"),
                    CornerRadius = 15,
                    HeightRequest = 50,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 280,
                    Margin = new Thickness(0, 50, 0, 20),
                };
                testButton.Clicked += OnStartTestClicked;
                ContentStack.Children.Add(testButton);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить тему: {ex.Message}", "OK");
            }
        }

        // ️ Сигнатура должна быть ИМЕННО такой:
        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Кнопка теста создаётся динамически из кода
        private async void OnStartTestClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Тест", $"Запуск теста для темы {_topicNumber}", "OK");
        }
    }
}