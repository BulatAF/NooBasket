using Microsoft.Maui.Controls;
using System.Text.Json;
using NooBasket.Models;

namespace NooBasket
{
    public partial class TopicDetailPage : ContentPage
    {
        private int _topicNumber;

        public TopicDetailPage(int topicNumber)
        {
            InitializeComponent();   
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTopicContent();
        }

        private async Task LoadTopicContent()
        {
            try
            {
                // 1. Имя файла, который мы создали
                string fileName = "topic1.json";

                // 2. Открываем файл из ресурсов
                using var stream = await FileSystem.Current.OpenAppPackageFileAsync(fileName);
                using var reader = new System.IO.StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                // 3. Превращаем текст JSON в объекты C#
                var topicData = JsonSerializer.Deserialize<TopicData>(json);

                // 4. Показываем заголовок
                if (topicData != null)
                {
                    TopicTitleLabel.Text = topicData.Title;

                    // 5. В цикле добавляем текст НА ПЕРВОЕ место (перед кнопкой)
                    int insertIndex = 0; // Начинаем вставлять с начала

                    foreach (var block in topicData.Blocks)
                    {
                        if (block.Type == "text")
                        {
                            var label = new Label
                            {
                                Text = block.Text,
                                TextColor = Colors.Black,
                                FontSize = 18,
                                LineBreakMode = LineBreakMode.WordWrap
                            };

                            // Вставляем ПЕРЕД кнопкой, а не после
                            ContentStack.Children.Insert(insertIndex, label);
                            insertIndex++;
                        }
                    }
                }
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

        private async void OnStartTestClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Тест", $"Запуск теста для темы {_topicNumber}", "OK");
        }
    }
}