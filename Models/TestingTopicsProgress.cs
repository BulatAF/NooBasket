using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.Models
{
    public class TestingTopicsProgress
    {
        public int NumberOfCorrect { get; set; }
        public int NumberOfAll { get; set; }
        public List<int> Answers { get; set; } = new();

        public double Percent
        {
            get
            {
                if (NumberOfAll > 0)
                {
                    double percent = (double)NumberOfCorrect / NumberOfAll * 100;
                    return Math.Round(percent, 0); // округляем до целого
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}