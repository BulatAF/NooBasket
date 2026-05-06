using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;

namespace NooBasket.ViewModels
{
    [QueryProperty(nameof(TopicId), "topicId")]
    public partial class TestingResultPageViewModel : ObservableObject
    {
        // процент правильных ответов 
        [ObservableProperty]
        private string _percent = "0%";

        // строка с количеством правильных ответов 
        [ObservableProperty]
        private string _correctCount = "0 из 0";

        // номер темы 
        private int _topicId;

        // свойство для получения номера темы, при установке загружаем результаты
        public int TopicId
        {
            get => _topicId;
            set
            {
                _topicId = value;
                LoadResultsAsync(); // когда получили id загружаем результаты
            }
        }

        // загружаем результаты теста из файла
        private async void LoadResultsAsync()
        {
            try
            {
                // получаем прогресс по теме из файла TestingProgress.json
                TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(_topicId);

                // получаем тему чтобы узнать общее количество вопросов
                TestTopic? topic = await TestingTopicsLoader.GetTopicAsync(_topicId);

                // если прогресс и тема существуют
                if (progress != null && topic != null)
                {
                    // форматируем процент с двумя знаками после запятой
                    Percent = $"{progress.Percent:F2}%";

                    // показываем сколько правильных из скольки всего
                    CorrectCount = $"{progress.NumberOfCorrect} из {topic.Questions.Count}";

                    // убеждаемся что прогресс точно сохранен в файл
                    await TestingTopicsProgressLoader.UpdateProgressAsync(_topicId, progress);
                }
            }
            catch (Exception ex)
            {
                // если чтото пошло не так
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        // команда для возврата в меню тестов
        [RelayCommand]
        private async Task GoToTestingPageAsync()
        {
            await Shell.Current.GoToAsync("///TestingPage");
        }
    }
}