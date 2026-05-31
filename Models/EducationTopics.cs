using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

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
        public string TitleWithID => $"{Id}. {Title}"; //чтобы выводились темы в формате номер темы. Тема 

        [JsonPropertyName("blocks")]
        public List<Block> Blocks { get; set; } = new();

        // доступность темы (не сохраняется в json)
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