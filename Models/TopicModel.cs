namespace NooBasket.Models
{
    public class TopicData
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<TopicBlock> Blocks { get; set; } = new();
    }
    public class TopicBlock
    {
        public string Type { get; set; } = "text"; // "text" | "image"
        public string? Text { get; set; }
        public string? Image { get; set; } // filename in Resources/Images (e.g. "my_pic.png")
        public string? Caption { get; set; }
    }
}