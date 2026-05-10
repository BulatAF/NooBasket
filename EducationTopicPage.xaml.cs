using NooBasket.ViewModels;
using NooBasket.Models;

namespace NooBasket;

public partial class EducationTopicPage : ContentPage
{
    public EducationTopicPage()
    {
        InitializeComponent();
        BindingContext = new EducationTopicPageViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is EducationTopicPageViewModel vm)
        {
            await Task.Delay(100); // ждем загрузки данных (костыль)
            // чистим контейнер чтобы не дублировать контент при возврате на страницу
            BlocksContainer.Children.Clear();

            // перебираем каждый блок из загруженного списка
            foreach (Block block in vm.Blocks)
            {
                // создаем вертикальный стек для одного элемента (текст+картинка+подпись)
                VerticalStackLayout stack = new VerticalStackLayout { Spacing = 5, Padding = 0 };

                // если в блоке есть текст добавляем его
                if (!string.IsNullOrEmpty(block.Text))
                {
                    stack.Children.Add(new Label
                    {
                        Text = block.Text,
                        FontSize = 22 
                    });
                }

                // если есть ссылка на картинку - добавляем изображение
                if (!string.IsNullOrEmpty(block.Image))
                {
                    stack.Children.Add(new Image
                    {
                        Source = block.Image,
                        HeightRequest = 200,
                        HorizontalOptions = LayoutOptions.Center
                    });
                }

                // если есть подпись к картинке добавляем её курсивом
                if (!string.IsNullOrEmpty(block.Caption))
                {
                    stack.Children.Add(new Label
                    {
                        Text = block.Caption,
                        FontSize = 16,
                        HorizontalOptions = LayoutOptions.Center
                    });
                }

                // добавляем собранный элемент в общий контейнер
                BlocksContainer.Children.Add(stack);
            }
        }
    }
}