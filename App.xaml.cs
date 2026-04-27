namespace NooBasket
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Глобальная обработка необработанных исключений
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Current.MainPage.DisplayAlert("Критическая ошибка", ex?.Message ?? "Неизвестная ошибка", "OK");
                });
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Current.MainPage.DisplayAlert("Ошибка задачи", e.Exception.Message, "OK");
                });
                e.SetObserved();
            };
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}