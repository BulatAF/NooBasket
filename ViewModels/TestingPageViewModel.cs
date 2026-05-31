using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    public partial class TestingPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<TestTopic> _topics = new();//список всех тем для тестирования

        public TestingPageViewModel()
        {
            LoadTopics();//при создании страницы сразу загружаем все темы
        }

        private async void LoadTopics()
        {
            try
            {
                List<TestTopic> tempList = new List<TestTopic>();

                for (int i = 1; i <= 19; i++)
                {
                    TestTopic topic = await TestingTopicsLoader.GetTopicAsync(i);
                    if (topic != null)
                    {
                        // проверяем доступность теста
                        topic.IsAvailable = await IsTopicAvailable(topic);
                        tempList.Add(topic);
                    }
                }

                Topics = tempList;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка загрузки тестов", ex.Message, "OK");
            }
        }

        // проверяем доступен ли тест
        private async Task<bool> IsTopicAvailable(TestTopic topic)
        {
            // первый тест всегда доступен
            if (topic.Id == 1)
                return true;

            // проверяем прогресс по предыдущему тесту
            int previousTopicId = topic.Id - 1;
            TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(previousTopicId);

            // доступен если предыдущий тест пройден на 70% или выше
            return progress != null && progress.Percent >= 70;
        }

        [RelayCommand]
        private async Task GoToMainAsync()//возвращение в главное меню
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        public async Task GoToTestingTopicAsync(TestTopic topic)
        {
            try
            {
                // если тест недоступен - показываем сообщение
                if (!topic.IsAvailable)
                {
                    int previousTopicId = topic.Id - 1;
                    TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(previousTopicId);

                    string lastTopicResult = "тест не пройден";
                    if (progress != null)
                    {
                        lastTopicResult = $"{progress.Percent}%";
                    }

                    await Shell.Current.DisplayAlert(
                        "Доступ ограничен",
                        $"Чтобы открыть тест \"{topic.Title}\", необходимо успешно пройти тест по предыдущей теме (на 70% или выше)." + "\n" + "\n" +
                        $"Текущий результат предыдущего теста: {lastTopicResult}",
                        "OK"
                    );
                    return;
                }

                await Shell.Current.GoToAsync($"//TestingTopicPage?topicId={topic.Id}&returnRoute=TestingPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из меню тестирования", ex.Message, "OK");
            }
        }
    }
}