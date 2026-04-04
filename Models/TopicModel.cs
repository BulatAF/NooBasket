namespace NooBasket.Models
{
    public class TopicData
    {
        public string Title { get; set; }
        public List<TopicBlock> Blocks { get; set; }
    }
    public class TopicBlock
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }
}