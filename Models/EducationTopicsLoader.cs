using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NooBasket.Models
{
    public class EducationTopicsLoader
    {
        private static EducationTopics? _allTopics;
        // Task - означает что функция точно вернет какой то результат когда завершится
        // async - помечает что программа может продолжать работу пока функция еще не завершилась
        // await - пока файл загружается программа не ждет а продолжает выполнять другие действия
        private static async Task LoadAllTopics()
        {
            if (_allTopics != null) return;//если файл уже загрузили раньше

            using Stream stream = await FileSystem.OpenAppPackageFileAsync("EducationTopics.json");
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string jsonContent = await reader.ReadToEndAsync();// читаем из файла и преобразуем все в строку
            _allTopics = JsonSerializer.Deserialize<EducationTopics>(jsonContent);// берем строку и с помощью Deserialize приводим ее к типу 
        }


        public static async Task<Topic?> GetTopicAsync(int topicId)// Task<Topic?> значит что функция вернет Topic
        {
            await LoadAllTopics();

            if (_allTopics == null)
            {
                return null;
            }

            foreach (Topic topic in _allTopics.Topics)// проходим по всем темам ищем нужную
            {
                if (topic.Id == topicId)
                {
                    return topic;
                }
            }
            return null;
        }
    }
}