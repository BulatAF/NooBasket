using Microsoft.Maui.Platform;
using NooBasket.Models;
using NooBasket.ViewModels;
using System.ComponentModel;

namespace NooBasket;

public partial class EducationTopicPage : ContentPage
{
    private EducationTopicPageViewModel? _viewModel;

    public EducationTopicPage()
    {
        InitializeComponent();
        BindingContext = new EducationTopicPageViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        MainScrollView.ScrollToAsync(0, 0, false);

        if (BindingContext is EducationTopicPageViewModel vm)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            _viewModel = vm;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            LoadBlocks(_viewModel.Blocks);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(EducationTopicPageViewModel.Blocks))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_viewModel != null)
                {
                    LoadBlocks(_viewModel.Blocks);
                }
            });
        }
    }

    private void LoadBlocks(List<Block> blocks)
    {
        BlocksContainer.Children.Clear();

        foreach (Block block in blocks)
        {
            VerticalStackLayout stack = new VerticalStackLayout { Spacing = 5, Padding = 0 };

            if (!string.IsNullOrEmpty(block.Text))
            {
                stack.Children.Add(new Label
                {
                    Text = block.Text,
                    FontSize = 18,
                    LineBreakMode = LineBreakMode.WordWrap
                });
            }

            if (!string.IsNullOrEmpty(block.Image))
            {
                stack.Children.Add(new Image
                {
                    Source = block.Image,
                    HeightRequest = 200,
                    HorizontalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFit
                });
            }

            if (!string.IsNullOrEmpty(block.Caption))
            {
                stack.Children.Add(new Label
                {
                    Text = block.Caption,
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Center,
                    LineBreakMode = LineBreakMode.WordWrap
                });
            }

            BlocksContainer.Children.Add(stack);
        }

        MainScrollView.ScrollToAsync(0, 0, false);
    }
}