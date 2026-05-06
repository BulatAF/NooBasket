using CommunityToolkit.Mvvm.ComponentModel;

namespace NooBasket.Models
{
    public partial class TopicStatistics : ObservableObject
    {
        [ObservableProperty]
        private int _topicId;

        [ObservableProperty]
        private string _topicTitle = string.Empty;

        [ObservableProperty]
        private bool _isCompleted = false;

        [ObservableProperty]
        private double _percent = 0;

        [ObservableProperty]
        private int _correctCount = 0;

        [ObservableProperty]
        private int _totalQuestions = 0;

        public string StatsText
        {
            get
            {
                if (!IsCompleted)
                    return "Тест ещё не был пройден";

                return $"Результат: {Percent:F1}% ({CorrectCount} из {TotalQuestions})";
            }
        }

        public Color StatsColor
        {
            get
            {
                if (!IsCompleted)
                    return Colors.Gray;

                if (Percent >= 80)
                    return Colors.Green;
                else if (Percent >= 60)
                    return Colors.Orange;
                else
                    return Colors.Red;
            }
        }

        public double ProgressValue
        {
            get
            {
                if (!IsCompleted || TotalQuestions == 0)
                    return 0;

                return Percent / 100;
            }
        }
    }
}