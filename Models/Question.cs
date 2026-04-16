using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NooBasket.Models
{
    class Question
    {
        public string Text { get; set; }
        public string[] Various { get; set; }
        public int CorrectAnswer { get; set; }
        public Question()
        {
            Text = "404?";
            Various = [ "401", "402", "403"];
            CorrectAnswer = 2;
        }
        public Question(string text, string[] various, int correctAnswer)
        {
            Text = text;
            Various = various;
            CorrectAnswer = correctAnswer;
        }
    }
}
