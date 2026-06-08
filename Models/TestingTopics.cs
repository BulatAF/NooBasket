using System.Text.Json.Serialization;
using Microsoft.Maui.Graphics;

namespace NooBasket.Models
{
    public class TestingTopics
    {
        [JsonPropertyName("topics")]
        public List<TestTopic> Topics { get; set; } = new();
    }

    public class TestTopic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        public string TitleWithId => $"{Id}. {Title}";

        [JsonPropertyName("questions")]
        public List<TestQuestion> Questions { get; set; } = new();

        // доступность теста (не сохраняется в json)
        public bool IsAvailable { get; set; } = true;

        // цвет кнопки в зависимости от доступности
        public Color ButtonColor
        {
            get
            {
                if (IsAvailable)
                {
                    return Color.FromArgb("#E28F04");
                }
                else
                {
                    return Colors.LightGray;
                }
            }
        }
    }

    public class TestQuestion
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("various")]
        public List<string> Various { get; set; } = new();

        [JsonPropertyName("answerText")]
        public string AnswerText { get; set; } = string.Empty;
    }
}