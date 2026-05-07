using System.Text.Json;

namespace NooBasket.Models
{
    public static class TestingTopicsProgressLoader
    {
        private static Dictionary<int, TestingTopicsProgress>? _allProgress; //передаем ключ - id темы и значение - прогресс

        private static string _filePath = Path.Combine(FileSystem.AppDataDirectory, "TestingProgress.json");
        //создаем файл в котором будем хранить статистику, FileSystem.AppDataDirectory - место в памяти телефона, которое выделено под приложение
        //Path.Combine - создает путь к файлу

        private static async Task LoadAllProgress()
        {
            if (_allProgress != null) return; //прогресс уже загружен

            if (File.Exists(_filePath)) //проверяем существует ли файл
            {
                string json = await File.ReadAllTextAsync(_filePath); //читаем весь текст из файла

                //пытаемся превратить json строку в словарь
                Dictionary<int, TestingTopicsProgress>? progress = JsonSerializer.Deserialize<Dictionary<int, TestingTopicsProgress>>(json);

                if (progress != null) //если десериализация прошла успешно
                {
                    _allProgress = progress;
                }
                else //если файл пустой или повреждён
                {
                    _allProgress = new Dictionary<int, TestingTopicsProgress>(); //создаем новый пустой словарь
                }
            }
            else //если файла не существует
            {
                _allProgress = new Dictionary<int, TestingTopicsProgress>(); //создаем новый пустой словарь
            }
        }

        //получаем прогресс по id темы
        public static async Task<TestingTopicsProgress?> GetProgressAsync(int topicId)
        {
            await LoadAllProgress(); //сначала загружаем прогресс из файла

            if (_allProgress == null) return null; //если прогресса нет - возвращаем null

            if (_allProgress.ContainsKey(topicId)) //проверяем есть ли в словаре такая тема
                return _allProgress[topicId]; //возвращаем прогресс для этой темы

            return null; //темы нет в словаре
        }

        //обновить прогресс для темы
        public static async Task UpdateProgressAsync(int topicId, TestingTopicsProgress progress)
        {
            await LoadAllProgress(); //сначала загружаем прогресс из файла

            if (_allProgress == null) return; //если прогресса нет - выходим

            _allProgress[topicId] = progress; //добавляем или обновляем прогресс для темы

            //превращаем словарь в json строку с отступами (чтобы файл был читаемый)
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };//добавляем настройки с которыми все запишется в файл
            //WriteIndented = true значит что в json файле все будет красиво с отступами
            string json = JsonSerializer.Serialize(_allProgress, options); //превращаем словаь в строку json

            await File.WriteAllTextAsync(_filePath, json); //открываем файл, записываем json строку и закрываем
        }
    }
}