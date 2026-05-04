using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;

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
                await Shell.Current.GoToAsync($"///EducationTopicPage?topicId={topic.Id}");//переходим на страницу темы по ее номеру
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из меню обучения", ex.Message, "OK");
            }
        }
    }
}