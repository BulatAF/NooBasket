using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.ViewModels
{
    [QueryProperty(nameof(TopicId), "topicId")]
    internal partial class TopicPageViewModel: ObservableObject
    {
        [ObservableProperty]
        private string _title = ""; // заголовок темы

        [ObservableProperty]
        private List<Block> _blocks = new List<Block>(); //текст+картинки для страницы

        private int _topicId;

        public int TopicId
        {
            get => _topicId;
            set
            {
                _topicId = value;
                LoadTopicAsync();
            }
        }
        private async void LoadTopicAsync()
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
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("//EducationPage");

        }

        [RelayCommand]
        private async Task GoToTest()
        {
            await Shell.Current.DisplayAlert("Тест", "Тест пройден", "Ок");
        }
    }
}
