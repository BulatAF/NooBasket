using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    [QueryProperty(nameof(ReturnToTopicId), "returnToTopicId")]
    [QueryProperty(nameof(SavedCorrect), "savedCorrect")]
    [QueryProperty(nameof(SavedTotal), "savedTotal")]
    public partial class StatisticsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _statisticsMessage = "В статистике представлены ваши лучшие результаты по каждому из тестов";

        [ObservableProperty]
        private List<TopicStatistics> _topicsStats = new();

        [ObservableProperty]
        private int _returnToTopicId = 0;

        [ObservableProperty]
        private int _savedCorrect = 0;

        [ObservableProperty]
        private int _savedTotal = 0;

        [ObservableProperty]
        private bool _isBackButtonVisible = false;

        [ObservableProperty]
        private bool _isMainButtonVisible = true;

        partial void OnReturnToTopicIdChanged(int value)
        {
            if (value > 0)
            {
                IsBackButtonVisible = true;
                IsMainButtonVisible = true;
            }
            else
            {
                IsBackButtonVisible = false;
                IsMainButtonVisible = true;
            }
        }

        public StatisticsPageViewModel()
        {
            LoadStatistics();
        }

        public async void LoadStatistics()
        {
            try
            {
                List<TopicStatistics> tempStats = new List<TopicStatistics>();

                for (int i = 1; i <= 19; i++)
                {
                    TestTopic? topic = await TestingTopicsLoader.GetTopicAsync(i);
                    TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(i);

                    TopicStatistics stat = new TopicStatistics();
                    stat.TopicId = i;

                    if (topic != null)
                    {
                        stat.TopicTitle = topic.Title;
                    }
                    else
                    {
                        stat.TopicTitle = $"Тема {i}";
                    }

                    if (progress != null && progress.NumberOfAll > 0)
                    {
                        stat.IsCompleted = true;
                        stat.CorrectCount = progress.NumberOfCorrect;
                        stat.TotalQuestions = progress.NumberOfAll;
                        stat.Percent = progress.Percent;
                    }
                    else
                    {
                        stat.IsCompleted = false;
                        if (topic != null)
                        {
                            stat.TotalQuestions = topic.Questions.Count;
                        }
                        else
                        {
                            stat.TotalQuestions = 0;
                        }
                    }

                    tempStats.Add(stat);
                }

                TopicsStats = tempStats;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task GoToMainAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        private async Task GoBackToResultsAsync()
        {
            if (ReturnToTopicId > 0)
            {
                // передаем сохраненные результаты обратно
                var navigationParams = new Dictionary<string, object>
                {
                    { "topicId", ReturnToTopicId },
                    { "lastAttemptCorrect", SavedCorrect },
                    { "lastAttemptTotal", SavedTotal }
                };
                await Shell.Current.GoToAsync($"//TestingResultPage", navigationParams);
            }
        }
    }
}