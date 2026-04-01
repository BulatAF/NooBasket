using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Graphics.Canvas.Text;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace NooBasket.ViewModels
{
    public partial class MainViewModels : ObservableObject
    {
        [ObservableProperty]
        private Tests test = new Tests("Программист ставит на тумбочку перед сном два стакана. " +
            "Один с водой — на случай, если захочет пить. А зачем второй, пустой?", 
            ["Для зубных протезов", "Для симметрии", "На случай, если НЕ захочет пить", "Чтобы жена не ругалась"], 2);

        [ObservableProperty]
        private Color newColor1 = Colors.Blue;

        [ObservableProperty]
        private Color newColor2 = Colors.Blue;

        [ObservableProperty]
        private Color newColor3 = Colors.Blue;

        [ObservableProperty]
        private Color newColor4 = Colors.Blue;

        [RelayCommand]
        private void ReplayColor1()
        {
            NewColor1 = NewColor2 = NewColor3 = NewColor4 = Colors.Blue;
            if (test.CorrectAnswer == 0)
                NewColor1 = Colors.Green;
            else NewColor1 = Colors.Red;
        }
        [RelayCommand]
        private void ReplayColor2()
        {
            NewColor1 = NewColor2 = NewColor3 = NewColor4 = Colors.Blue;
            if (test.CorrectAnswer == 1)
                NewColor2 = Colors.Green;
            else NewColor2 = Colors.Red;
        }
        [RelayCommand]
        private void ReplayColor3()
        {
            NewColor1 = NewColor2 = NewColor3 = NewColor4 = Colors.Blue;
            if (test.CorrectAnswer == 2)
                NewColor3 = Colors.Green;
            else NewColor3 = Colors.Red;
        }
        [RelayCommand]
        private void ReplayColor4()
        {
            NewColor1 = NewColor2 = NewColor3 = NewColor4 = Colors.Blue;
            if (test.CorrectAnswer == 3)
                NewColor4 = Colors.Green;
            else NewColor4 = Colors.Red;
        }
    }
}
