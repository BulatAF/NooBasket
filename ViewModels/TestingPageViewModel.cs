using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    public partial class TestingPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<TestTopic> _topics = new();//список всех тем для тестирования (класс TestTopic - файл Models\TestingTopics.cs)

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

        [RelayCommand]
        private async Task GoToMainAsync()//возвращение в главное меню
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        //команда для перехода на страницу тестирования по конкретной теме
        //принимает объект TestTopic которую выбрал пользователь
        [RelayCommand]
        public async Task GoToTestingTopicAsync(TestTopic topic)//передаем тему
        {
            try
            {
                // если это первая тема - доступна всегда
                if (topic.Id == 1)
                {
                    await Shell.Current.GoToAsync($"///TestingTopicPage?topicId={topic.Id}");
                    return;
                }

                // проверяем прогресс по предыдущей теме 
                int previousTopicId = topic.Id - 1;
                TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(previousTopicId);

                // если предыдущий тест не пройден или результат меньше 70%
                if (progress == null || progress.Percent < 70)
                {
                    string lastTopicResult = "тест не пройден";
                    if (progress != null)
                    {
                        lastTopicResult = $"{progress.Percent:F1}%";
                    }

                    await Shell.Current.DisplayAlert(
                        "Доступ ограничен",
                        $"Чтобы открыть тест \"{topic.Title}\", необходимо пройти предыдущий тест на 70% или выше." + "\n" + "\n" +
                        $"Текущий результат предыдущего теста: {lastTopicResult}",
                        "OK"
                    );
                    return;
                }

                // если проверка пройдена - переходим
                await Shell.Current.GoToAsync($"///TestingTopicPage?topicId={topic.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из меню тестирования", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task OpenHelpAsync()
        {
            await HelpService.OpenHelpAsync();
        }
    }
}