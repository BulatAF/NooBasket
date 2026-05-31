using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    [QueryProperty(nameof(TopicId), "topicId")]
    internal partial class EducationTopicPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "";

        [ObservableProperty]
        private List<Block> _blocks = new List<Block>();

        [ObservableProperty]
        private bool _isNextTopicButtonVisible = true;

        private int _topicId;

        public int TopicId
        {
            get => _topicId;
            set
            {
                _topicId = value;
                IsNextTopicButtonVisible = (_topicId < 19);
                _ = LoadTopicAsync();
            }
        }

        private async Task LoadTopicAsync()
        {
            try
            {
                var topic = await EducationTopicsLoader.GetTopicAsync(_topicId);
                if (topic != null)
                {
                    Title = topic.Title;
                    Blocks = topic.Blocks;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ошибка", $"Тема с ID {_topicId} не найдена", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить тему: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task GoToEducationPage()
        {
            await Shell.Current.GoToAsync("//EducationPage");
        }

        [RelayCommand]
        private async Task GoToTest()
        {
            if (_topicId <= 0) return;

            // первый тест всегда доступен
            if (_topicId == 1)
            {
                await Shell.Current.GoToAsync($"//TestingTopicPage?topicId={_topicId}&returnRoute=EducationTopicPage");
                return;
            }

            // проверяем все предыдущие тесты
            bool allPreviousPassed = true;
            int firstFailedTopicId = 0;
            string firstFailedTopicTitle = "";
            int failedTopicPercent = 0;

            for (int i = 1; i < _topicId; i++)
            {
                TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(i);

                if (progress == null || progress.Percent < 70)
                {
                    allPreviousPassed = false;
                    firstFailedTopicId = i;

                    TestTopic? failedTopic = await TestingTopicsLoader.GetTopicAsync(i);
                    firstFailedTopicTitle = failedTopic?.Title ?? $"Тема {i}";
                    failedTopicPercent = (int)Math.Round(progress?.Percent ?? 0);
                    break;
                }
            }

            if (allPreviousPassed)
            {
                await Shell.Current.GoToAsync($"//TestingTopicPage?topicId={_topicId}&returnRoute=EducationTopicPage");
            }
            else
            {
                bool goToTesting = await Shell.Current.DisplayAlert(
                    "Тест недоступен",
                    $"Чтобы пройти тест по теме \"{Title}\", необходимо успешно завершить все предыдущие тесты (на 70% или выше).\n\n" +
                    $"Первый непройденный тест: \"{firstFailedTopicTitle}\"\n" +
                    $"Ваш результат: {failedTopicPercent}%",
                    "Перейти к меню тестов",
                    "Отмена"
                );

                if (goToTesting)
                {
                    await Shell.Current.GoToAsync("//TestingPage");
                }
            }
        }

        [RelayCommand]
        private async Task GoToNextEducationTopicAsync()
        {
            int nextTopicId = TopicId + 1;
            await Shell.Current.GoToAsync($"//EducationTopicPage?topicId={nextTopicId}");
        }
    }
}