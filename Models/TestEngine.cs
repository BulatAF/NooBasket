using Microsoft.Maui.Storage;
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

        public async Task LoadFromJson(string nameJSON)
        {
            using var stream = await FileSystem.Current.OpenAppPackageFileAsync(nameJSON);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            List<Question>? questions = await JsonSerializer.DeserializeAsync<List<Question>>(stream, options);
            Questions = questions ?? new List<Question>();
            UserAnswers = new int[Questions.Count];
            Array.Fill(UserAnswers, -1);
        }
    }
}