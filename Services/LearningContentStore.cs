using System.Text.Json;
using NooBasket.Models;

namespace NooBasket.Services
{
    /// <summary>
    /// Очень простой "магазин" контента.
    /// Мы храним все темы в одном JSON-файле внутри приложения и читаем его, когда нужно.
    /// </summary>
    public static class LearningContentStore
    {
        // Файл лежит в Resources/Raw и попадает в пакет приложения как MauiAsset
        private const string FileName = "learning_topics.json";

        /// <summary>
        /// Загружает все темы обучения из JSON.
        /// </summary>
        public static async Task<LearningContent> LoadAsync()
        {
            // 1) Открываем файл из пакета приложения
            using var stream = await FileSystem.Current.OpenAppPackageFileAsync(FileName);
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            // 2) Парсим JSON в C#-объекты
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            LearningContent? content = JsonSerializer.Deserialize<LearningContent>(json, options);

            // 3) Если файл пустой/битый — возвращаем "пустой" объект, чтобы не падать
            return content ?? new LearningContent();
        }
    }
}

