using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    public partial class EducationPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Topic> _topics = new List<Topic>();//инициализируем список из всех тем

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
                    Topic topic = await EducationTopicsLoader.GetTopicAsync(i);
                    if (topic != null)
                    {
                        tmpList.Add(topic);
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
        public async Task GoToEducationTopicAsync(Topic topic)
        {
            try
            {
                // используем абсолютный маршрут с двумя слешами и параметром
                await Shell.Current.GoToAsync($"//EducationTopicPage?topicId={topic.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из меню обучения", ex.Message, "OK");
            }
        }
    }
}