using System.Diagnostics;

namespace NooBasket.Services
{
    public static class HelpService
    {
        public static async Task OpenHelpAsync()
        {
            try
            {
                //путь к файлу справки в папке приложения
                string targetPath = Path.Combine(FileSystem.AppDataDirectory, "NooBasket2.chm");

                //если файла нет в папке приложения
                if (!File.Exists(targetPath))
                {
                    //открываем файл из ресурсов приложения
                    using Stream stream = await FileSystem.OpenAppPackageFileAsync("NooBasket2.chm");
                    //создаем файл в папке приложения
                    using FileStream fileStream = File.Create(targetPath);
                    //копируем содержимое
                    await stream.CopyToAsync(fileStream);
                }

                //открываем chm файл через 
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                //показываем ошибку если не удалось открыть справку
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось открыть справку: {ex.Message}", "OK");
            }
        }
    }
}