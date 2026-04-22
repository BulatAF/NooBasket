using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.ViewModels
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty] //атрибут, генерируется публичное свойство
        private string _welcomeMessage = "Добро пожаловать в NooBasket!";


        [RelayCommand] //генерируется команда для привязки к кнопке
        private async Task GoToEducationAsync()
        {
            await Shell.Current.GoToAsync("//EducationPage");
        }

        [RelayCommand]
        private async Task GoToTestingAsync()
        {
            await Shell.Current.GoToAsync("//TestingPage");
        }

        [RelayCommand]
        private async Task ContactSupportAsync()
        {
            await Shell.Current.DisplayAlert("Поддержка",
                "Свяжитесь с нами: тут адрес почты",
                "OK");
        }
    }
}
