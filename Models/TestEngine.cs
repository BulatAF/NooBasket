using NooBasket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NooBasket
{
    class TestEngine
    {
        List<Question>? _questions;
        int[]? _userAnswers = null;
        int _current = 0;

        public async Task LoadFromJson(string nameJSON)
        {
            //Path.Combine(FileSystem.AppDataDirectory, nameJSON);
            using (FileStream fs = new FileStream("C:\\Users\\Серёжа Родионов\\Desktop\\" +
                "Учёба\\Курсовой проект\\NooBasket\\Models\\" + nameJSON, FileMode.Open))
            {
                List<Question>? questions = await JsonSerializer.DeserializeAsync<List<Question>>(fs);
                _questions = questions ?? new List<Question>();
                _userAnswers = new int[_questions.Count];
                Array.Fill(_userAnswers, -1);
            }
        }

        public string? PrintQuestion(int id)
        {
            return _questions[id]?.Text;
        }
        public string[]? PrintVarAnswer(int id)
        {
            return _questions[id]?.Various;
        }
    }
}