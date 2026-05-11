using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _welcomeMessage = "Добро пожаловать в NooBasket!";//приветственное сообщение

        [RelayCommand]
        private async Task GoToEducationAsync()//переход в меню обучения
        {
            await Shell.Current.GoToAsync("//EducationPage");
        }

        [RelayCommand]
        private async Task GoToTestingAsync()//переход в меню тестов
        {
            await Shell.Current.GoToAsync("//TestingPage");
        }

        [RelayCommand]
        private async Task GoToStatisticsAsync()//переход на страницу с статистикой
        {
            await Shell.Current.GoToAsync("//StatisticsPage");
        }

        [RelayCommand]
        private async Task ContactSupportAsync()//всплывающее окно с данными техподдержки
        {
            await Shell.Current.DisplayAlert("Поддержка",
                "Свяжитесь с нами: ",
                "OK");
        }

        [RelayCommand]
        private async Task OpenHelpAsync()
        {
            await HelpService.OpenHelpAsync();
        }

    }
}