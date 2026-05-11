using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    public partial class EducationPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Topic> _topics = new List<Topic>();//инициализируем список из всех тем (класс Topic - файл Models\EducationTopics.cs)

        public EducationPageViewModel()
        {
            LoadTopics();
        }

        private async void LoadTopics()
        {
            try
            {
                List<Topic> tmpList = new List<Topic>();

                for (int i = 1; i <= 19; i++)
                {
                    Topic topic = await EducationTopicsLoader.GetTopicAsync(i);//проходим по всем темам, загружаем данные темы по ее номеру (файл Models\EducationTopicsLoader.cs)
                    if (topic != null)
                    {
                        tmpList.Add(topic);//добавляем
                    }
                }

                Topics = tmpList;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }


        [RelayCommand]
        private async Task GoToMainAsync()//возвращение на главную
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        public async Task GoToEducationTopicAsync(Topic topic)//передаем тему
        {
            try
            {
                // если это первая тема - доступна всегда
                if (topic.Id == 1)
                {
                    await Shell.Current.GoToAsync($"///EducationTopicPage?topicId={topic.Id}");
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
                        $"Чтобы открыть тему \"{topic.Title}\", необходимо пройти предыдущую тему на 70% или выше." + "\n" + "\n" +
                        $"Текущий результат предыдущей темы: {lastTopicResult}",
                        "OK"
                    );
                    return;
                }

                // если проверка пройдена - переходим
                await Shell.Current.GoToAsync($"///EducationTopicPage?topicId={topic.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из меню обучения", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task OpenHelpAsync()
        {
            await HelpService.OpenHelpAsync();
        }
    }
}