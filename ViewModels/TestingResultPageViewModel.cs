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

        [RelayCommand]
        public async Task GoToTestingTopicAgainAsync()
        {
            try
            {
                //передаем параметр topicId
                await Shell.Current.GoToAsync($"///TestingTopicPage?topicId={TopicId}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из страницы с результатом теста", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task GoToNextEducationTopicAsync()
        {
            try
            {
                if (TopicId != 19)
                {
                    int nextTopicId = TopicId + 1;

                    // проверяем прогресс по текущей теме (которую только что прошли)
                    TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(TopicId);

                    // если текущий тест не пройден или результат меньше 70%
                    if (progress == null || progress.Percent < 70)
                    {
                        string currentTopicResult = $"{progress.Percent:F1}%";

                        await Shell.Current.DisplayAlert(
                            "Доступ ограничен",
                            $"Чтобы перейти к следующей теме, необходимо пройти текущий тест на 70% или выше." + "\n" + "\n" +
                            $"Ваш результат: {currentTopicResult}",
                            "OK"
                        );
                        return;
                    }

                    // если проверка пройдена - переходим к следующей теме
                    await Shell.Current.GoToAsync($"///EducationTopicPage?topicId={nextTopicId}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Поздравляем",
                                                    "Все темы для обучения и тесты к ним пройдены!",
                                                    "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из страницы с результатом теста", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task GoToStatisticsAsync()//переход на страницу с статистикой
        {
            await Shell.Current.GoToAsync("//StatisticsPage");
        }
    }
}