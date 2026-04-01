using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace NooBasket.ViewModels
{
    public partial class MainViewModels : ObservableObject
    {
        [ObservableProperty]
        private int passwordLenght = 8;

        [ObservableProperty]
        private string result = "Пароль появится здесь";

        [RelayCommand]
        private void GeneratePass()
        {
            string chars = "QwErTyUiOpAsDfGhJkLzXcVbNm1234567890";
            Random rand = new Random();
            Result = new string(Enumerable.Repeat(chars, PasswordLenght)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
}
