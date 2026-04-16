using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.Models
{
    class TestResult
    {
        public int NumberOfCorrect { get; set; }
        public int NumberOfAll { get; set; }
        public double Percent => (double)NumberOfCorrect/NumberOfAll * 100;
        public int[] Answers {  get; set; }
    }
}
