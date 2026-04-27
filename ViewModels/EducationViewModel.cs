using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NooBasket.ViewModels
{
    public partial class EducationViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Topic> _topics = new List<Topic>();

        public EducationViewModel()
        {
            LoadTopics();
        }

        private async void LoadTopics()
        {
            try
            {
                List<Topic> tempList = new List<Topic>();

                for (int i = 1; i <= 19; i++)
                {
                    Topic topic = await EducationTopicsLoader.GetTopicAsync(i);
                    if (topic != null)
                    {
                        tempList.Add(topic);
                    }
                }

                Topics = tempList;
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
        public async Task GoToTopicAsync(Topic topic)
        {
            try
            {
                await Shell.Current.GoToAsync($"///TopicPage?topicId={topic.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка навигации", ex.Message, "OK");
            }
        }
    }
}