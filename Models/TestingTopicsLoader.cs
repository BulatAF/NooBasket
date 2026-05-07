using System.Text;
using System.Text.Json;

namespace NooBasket.Models
{
    public static class TestingTopicsLoader
    {
        private static TestingTopics? _allTopics;

        private static async Task LoadAllTopics()
        {
            if (_allTopics != null) return;

            using Stream stream = await FileSystem.OpenAppPackageFileAsync("TestingTopics.json");
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string jsonContent = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(jsonContent))
            {
                return;
            }

            _allTopics = JsonSerializer.Deserialize<TestingTopics>(jsonContent);
        }

        public static async Task<TestTopic?> GetTopicAsync(int topicId)
        {
            await LoadAllTopics();

            if (_allTopics == null)
            {
                return null;
            }

            foreach (TestTopic topic in _allTopics.Topics)
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