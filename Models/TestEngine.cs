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
        public List<Question>? Questions;
        public int[]? UserAnswers = null;
        public int Current = 0;

        public async Task LoadFromJson(string nameJSON)
        {
            //Path.Combine(FileSystem.AppDataDirectory, nameJSON);
            using (FileStream fs = new FileStream("C:\\Users\\Серёжа Родионов\\Desktop\\" +
                "Учёба\\Курсовой проект\\NooBasket\\Models\\" + nameJSON, FileMode.Open))
            {
                List<Question>? questions = await JsonSerializer.DeserializeAsync<List<Question>>(fs);
                Questions = questions ?? new List<Question>();
                UserAnswers = new int[Questions.Count];
                Array.Fill(UserAnswers, -1);
            }
        }
    }
}