using Microsoft.Maui.Storage;
using NooBasket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NooBasket
{
    class TestEngine
    {
        public List<Question>? Questions;
        public Dictionary<string,TestResult>? Results;

        public async Task LoadFromJson(string nameJSON)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Читаем вопросы (они всегда в пакете, только чтение)
            using var stream = await FileSystem.Current.OpenAppPackageFileAsync(nameJSON);
            Questions = await JsonSerializer.DeserializeAsync<List<Question>>(stream, options)
                        ?? new List<Question>();

            // Читаем прогресс через глобальный метод (из AppData)
            Results = await LoadGlobalProgress();

            // Если для этого теста уже есть запись, проверяем размер массива Answers
            if (Results.ContainsKey(nameJSON))
            {
                var currentResult = Results[nameJSON];

                // Если массив null или его длина не совпадает с количеством вопросов
                if (currentResult.Answers == null || currentResult.Answers.Length != Questions.Count)
                {
                    // Создаем новый массив правильной длины
                    var correctedAnswers = new int[Questions.Count];

                    // Если там были какие-то старые ответы, копируем их
                    if (currentResult.Answers != null)
                    {
                        int lengthToCopy = Math.Min(currentResult.Answers.Length, Questions.Count);
                        Array.Copy(currentResult.Answers, correctedAnswers, lengthToCopy);
                    }

                    currentResult.Answers = correctedAnswers;
                }
            }
        }

        public async Task<Dictionary<string, TestResult>> LoadGlobalProgress()
        {
            string targetFile = Path.Combine(FileSystem.AppDataDirectory, "Progress.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string jsonText;

            // ЕСЛИ ФАЙЛ УЖЕ ЕСТЬ В ПАПКЕ ДАННЫХ — ЧИТАЕМ ЕГО
            if (File.Exists(targetFile))
            {
                jsonText = await File.ReadAllTextAsync(targetFile);
            }
            // ЕСЛИ НЕТ (ПЕРВЫЙ ЗАПУСК) — БЕРЕМ ИЗ ПАКЕТА
            else
            {
                using var stream = await FileSystem.Current.OpenAppPackageFileAsync("Progress.json");
                using var reader = new StreamReader(stream);
                jsonText = await reader.ReadToEndAsync();

                // Сразу сохраняем его в AppData, чтобы файл там появился
                await File.WriteAllTextAsync(targetFile, jsonText);
            }

            return JsonSerializer.Deserialize<Dictionary<string, TestResult>>(jsonText, options)
                   ?? new Dictionary<string, TestResult>();
        }
        public async Task SaveGlobalProgress(Dictionary<string, TestResult> results)
        {
            string targetFile = Path.Combine(FileSystem.AppDataDirectory, "Progress.json");
            var options = new JsonSerializerOptions { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(results, options);
            await File.WriteAllTextAsync(targetFile, jsonString);
        }
    }
}