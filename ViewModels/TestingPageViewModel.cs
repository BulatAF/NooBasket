using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;

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
        public async Task GoToTestingTopicAsync(TestTopic topic)
        {
            try
            {
                //передаем параметр topicId
                await Shell.Current.GoToAsync($"///TestingTopicPage?topicId={topic.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации из меню тестирования", ex.Message, "OK");
            }
        }
    }
}