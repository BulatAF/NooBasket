using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.ViewModels
{
    public class Tests
    {
        public string Question{ get; set; }

        public string[] Variants { get; set; }

        protected int _correct;
        public int CorrectAnswer { 
            get => _correct;
            set 
            {
                if (value < 0 && value >= Variants.Length)
                    throw new IndexOutOfRangeException("Неверный номер правильного ответа!");
                else
                    _correct = value;
            } 
        }
        public Tests()
        {
            Question = "Чем играют в баскетбол:";
            Variants = ["Клюшкой" , "Пистолетом", "Мячом", "Ракеткой"];
            CorrectAnswer = 2;
        }
        public Tests(string ques, string[] ans, int correct)
        {
            Question = ques;
            Variants = ans;
            CorrectAnswer = correct;
        }
    }
}
