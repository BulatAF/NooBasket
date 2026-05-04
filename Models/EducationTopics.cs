using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NooBasket.Models
{
    public class EducationTopics
    {
        [JsonPropertyName("topics")] //ищем topics в json и записываем в свойство Topics
        public List<Topic> Topics { get; set; } = new(); // записываем все в список, (new() пустой список по умолчанию)
    }
    public class Topic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;// по умолчанию пустая строка 
        public string TitleWithID=> $"{Id}. {Title}"; //чтобы выводились темы в формате номер темы. Тема 

        [JsonPropertyName("blocks")]
        public List<Block> Blocks { get; set; } = new();
    }

    public class Block
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string? Text { get; set; }// ? значит что текст, картинка или подпись к ней могут отстутствовать

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("caption")]
        public string? Caption { get; set; }
    }
}
