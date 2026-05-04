using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;

namespace NooBasket.ViewModels
{
    public partial class StatisticsPageViewModel : ObservableObject
    {
        // список статистики по всем темам
        [ObservableProperty]
        private List<TopicStatistics> _topicsStats = new();

        public StatisticsPageViewModel()
        {
            LoadStatistics(); // при создании страницы загружаем статистику
        }

        public async void LoadStatistics()
        {
            try
            {
                // временный список для сбора данных
                List<TopicStatistics> tempStats = new List<TopicStatistics>();

                // проходим по всем темам
                for (int i = 1; i <= 19; i++)
                {
                    // получаем тему чтобы узнать название
                    TestTopic? topic = await TestingTopicsLoader.GetTopicAsync(i);

                    // получаем прогресс по этой теме (если проходили)
                    TestingTopicsProgress? progress = await TestingTopicsProgressLoader.GetProgressAsync(i);

                    // создаем объект статистики для темы
                    TopicStatistics stat = new TopicStatistics();
                    stat.TopicId = i;
                    stat.TopicTitle = topic.Title;

                    // если прогресс есть и были ответы на вопросы
                    if (progress != null && progress.NumberOfAll > 0)
                    {
                        stat.IsCompleted = true; // тест пройден
                        stat.CorrectCount = progress.NumberOfCorrect; // сколько правильных
                        stat.TotalQuestions = progress.NumberOfAll; // сколько всего ответов
                        stat.Percent = progress.Percent; // процент правильных
                    }
                    // если прогресса нет (тест еще не проходили)
                    else
                    {
                        stat.IsCompleted = false; // тест еще не проходили

                        // общее количество вопросов в теме
                        if (topic != null)
                        {
                            stat.TotalQuestions = topic.Questions.Count;
                        }
                        else
                        {
                            stat.TotalQuestions = 0;
                        }
                    }

                    tempStats.Add(stat); // добавляем во временный список
                }

                TopicsStats = tempStats; // присваиваем основной список 
            }
            catch (Exception ex)
            {
                // если что то пошло не так
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        // команда для возврата на главную
        [RelayCommand]
        private async Task GoToMainAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}