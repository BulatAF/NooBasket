using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.ViewModels
{
    public partial class EducationViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task GoToMainAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        private async Task GoToTopic1Async()
        {

            await Shell.Current.GoToAsync("//Topic1Page");
        }

        [RelayCommand]
        private async Task GoToTopic2Async()
        {
            if (TopicAvailable.IsAvailable(2))
            {
                await Shell.Current.GoToAsync("//Topic2Page");
            }
            else
            {
                await Shell.Current.DisplayAlert("Тема закрыта",
                    "Сначала завершите Тему 1",
                    "Понятно");
            }
        }

    }
}
