using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    [QueryProperty(nameof(TopicId), "topicId")]
    [QueryProperty(nameof(LastAttemptCorrect), "lastAttemptCorrect")]
    [QueryProperty(nameof(LastAttemptTotal), "lastAttemptTotal")]
    public partial class TestingResultPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _percent = "0%";

        [ObservableProperty]
        private string _correctCount = "0 из 0";

        [ObservableProperty]
        private string _bestPercent = ""; // для отображения лучшего результата

        private int _topicId;
        private int _lastAttemptCorrect;
        private int _lastAttemptTotal;

        public int TopicId
        {
            get => _topicId;
            set
            {
                _topicId = value;
                LoadResultsAsync();
            }
        }

        public int LastAttemptCorrect
        {
            get => _lastAttemptCorrect;
            set
            {
                _lastAttemptCorrect = value;
                UpdateDisplay();
            }
        }

        public int LastAttemptTotal
        {
            get => _lastAttemptTotal;
            set
            {
                _lastAttemptTotal = value;
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            if (_lastAttemptTotal > 0)
            {
                double percentValue = (double)_lastAttemptCorrect / _lastAttemptTotal * 100;
                Percent = $"{Math.Round(percentValue, 0)}%";
                CorrectCount = $"{_lastAttemptCorrect} из {_lastAttemptTotal}";
            }
            else
            {
                Percent = "0%";
                CorrectCount = "0 из 0";
            }
        }

        private async void LoadResultsAsync()
        {
            try
            {
                TestTopic? topic = await TestingTopicsLoader.GetTopicAsync(_topicId);

                // загружаем лучший результат для отображения в интерфейсе 
                if (topic != null)
                {
                    TestingTopicsProgress? bestProgress = await TestingTopicsProgressLoader.GetProgressAsync(_topicId);
                    if (bestProgress != null && bestProgress.NumberOfAll > 0)
                    {
                        BestPercent = $"Лучший результат: {bestProgress.Percent}%";
                    }
                    else
                    {
                        BestPercent = "";
                    }
                }

                // если параметры последней попытки пришли - показываем их
                if (_lastAttemptTotal > 0)
                {
                    return;
                }

                // если параметров нет - берем лучший результат из файла
                if (topic != null)
                {
                    TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(_topicId);
                    if (progress != null && progress.NumberOfAll > 0)
                    {
                        _lastAttemptCorrect = progress.NumberOfCorrect;
                        _lastAttemptTotal = progress.NumberOfAll;
                        Percent = $"{progress.Percent}%";
                        CorrectCount = $"{progress.NumberOfCorrect} из {topic.Questions.Count}";
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task GoToTestingPageAsync()
        {
            await Shell.Current.GoToAsync("//TestingPage");
        }

        [RelayCommand]
        public async Task GoToTestingTopicAgainAsync()
        {
            try
            {
                await Shell.Current.GoToAsync($"//TestingTopicPage?topicId={TopicId}&returnRoute=TestingPage&returnToResults=yes&prevCorrect={_lastAttemptCorrect}&prevTotal={_lastAttemptTotal}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task GoToNextEducationTopicAsync()
        {
            try
            {
                // проверяем лучший результат, а не последний
                TestingTopicsProgress? bestProgress = await TestingTopicsProgressLoader.GetProgressAsync(TopicId);

                if (TopicId != 19)
                {
                    int nextTopicId = TopicId + 1;
                    await Shell.Current.GoToAsync($"//EducationTopicPage?topicId={nextTopicId}");
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
                await Shell.Current.DisplayAlert("Ошибка навигации", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task GoToStatisticsAsync()
        {
            await Shell.Current.GoToAsync($"//StatisticsPage?returnToTopicId={TopicId}&savedCorrect={_lastAttemptCorrect}&savedTotal={_lastAttemptTotal}");
        }

        [RelayCommand]
        private async Task GoToMainAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}