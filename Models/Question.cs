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
        public string AnswerText { get; set; }
        public Question()
        {
            Text = "404?";
            Various = [ "", "", ""];
            AnswerText = "ОШИБКА 404";
        }
    }
}
