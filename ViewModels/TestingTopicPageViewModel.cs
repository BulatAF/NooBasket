using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using Microsoft.Maui.Graphics;

namespace NooBasket.ViewModels
{
    // позволяет получить номер темы из строки навигации
    [QueryProperty(nameof(TopicId), "topicId")]
    public partial class TestingTopicPageViewModel : ObservableObject
    {
        // заголовок темы
        [ObservableProperty]
        private string _title = "";

        // текст вопроса
        [ObservableProperty]
        private string _questionText = "";

        // путь к картинке
        [ObservableProperty]
        private string? _questionImage;

        // показывать картинку или нет
        [ObservableProperty]
        private bool _isImageVisible = false;

        // список вариантов ответов
        [ObservableProperty]
        private List<string> _answerOptions = new List<string>();

        // объяснение после ответа
        [ObservableProperty]
        private string _answerExplanation = "Выберите вариант ответа";

        // текст на кнопке "Следующая" или "Завершить"
        [ObservableProperty]
        private string _nextButtonText = "Следующая";

        // ответил ли пользователь на вопрос
        [ObservableProperty]
        private bool _isAnswered = false;

        // номер текущего вопроса
        [ObservableProperty]
        private int _currentQuestionNumber = 1;

        // всего вопросов в теме
        [ObservableProperty]
        private int _totalQuestions = 0;

        // цвет фона первой кнопки
        [ObservableProperty]
        private Color _answer0Color = Colors.Purple;

        // цвет фона второй кнопки
        [ObservableProperty]
        private Color _answer1Color = Colors.Purple;

        // цвет фона третьей кнопки
        [ObservableProperty]
        private Color _answer2Color = Colors.Purple;

        // цвет фона четвертой кнопки
        [ObservableProperty]
        private Color _answer3Color = Colors.Purple;

        // цвет текста 
        [ObservableProperty]
        private Color _answer0TextColor = Colors.White;

        [ObservableProperty]
        private Color _answer1TextColor = Colors.White;

        [ObservableProperty]
        private Color _answer2TextColor = Colors.White;

        [ObservableProperty]
        private Color _answer3TextColor = Colors.White;

        // текущая тема 
        private TestTopic? _currentTopic;

        // индекс текущего вопроса 
        private int _currentQuestionIndex = 0;

        // правильный ответ на текущий вопрос
        private string _currentCorrectAnswer = "";

        // номер темы
        private int _topicId;

        // прогресс по теме (сколько правильных)
        private TestingTopicsProgress? _progress;

        // выбран ли ответ (чтобы нельзя было нажать дважды)
        private bool _answerSelected = false;

        // свойство для получения номера темы, при установке загружаем тему
        public int TopicId
        {
            get => _topicId;
            set
            {
                _topicId = value;
                LoadTopicAsync();
            }
        }

        // загружаем тему по номеру из файла TestingTopics.json
        private async void LoadTopicAsync()
        {
            try
            {
                _currentTopic = await TestingTopicsLoader.GetTopicAsync(_topicId);

                // если тема не найдена показываем ошибку
                if (_currentTopic == null)
                {
                    await Shell.Current.DisplayAlert("Ошибка", $"Тема {_topicId} не найдена", "OK");
                    return;
                }

                // сохраняем заголовок и количество вопросов
                Title = _currentTopic.Title;
                TotalQuestions = _currentTopic.Questions.Count;

                // создаем новый прогресс для этого теста (начинаем с нуля)
                _progress = new TestingTopicsProgress();
                _progress.Answers = new List<int>(new int[_currentTopic.Questions.Count]);
                _progress.NumberOfCorrect = 0;
                _progress.NumberOfAll = 0;

                // начинаем с первого вопроса
                _currentQuestionIndex = 0;
                LoadCurrentQuestion();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить тест: {ex.Message}", "OK");
            }
        }

        // загружаем текущий вопрос
        private void LoadCurrentQuestion()
        {
            // проверяем что тема существует и мы не вышли за пределы списка
            if (_currentTopic == null)
                return;

            if (_currentQuestionIndex >= _currentTopic.Questions.Count)
                return;

            // берем вопрос по индексу
            TestQuestion question = _currentTopic.Questions[_currentQuestionIndex];

            // заполняем текст вопроса
            QuestionText = question.Text;

            // номер вопроса для отображения (индекс + 1)
            CurrentQuestionNumber = _currentQuestionIndex + 1;

            // проверяем есть ли картинка у вопроса
            if (question.Image != null && question.Image != "")
            {
                QuestionImage = question.Image;
                IsImageVisible = true;
            }
            else
            {
                QuestionImage = null;
                IsImageVisible = false;
            }

            // берем варианты ответов из json
            List<string> variants = question.Various.ToList();

            // правильный ответ - первый в списке из json
            _currentCorrectAnswer = variants[0];

            // перемешиваем варианты ответов
            Random random = new Random();
            for (int i = variants.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                string temp = variants[i];
                variants[i] = variants[j];
                variants[j] = temp;
            }

            // сохраняем перемешанные варианты
            AnswerOptions = variants;

            // сбрасываем состояние для нового вопроса
            AnswerExplanation = "Выберите вариант ответа";
            IsAnswered = false;
            _answerSelected = false;

            // сбрасываем цвета кнопок
            ResetAnswerColors();

            // если это последний вопрос - меняем текст кнопки
            if (_currentQuestionIndex + 1 == _currentTopic.Questions.Count)
            {
                NextButtonText = "Завершить";
            }
            else
            {
                NextButtonText = "Следующая";
            }
        }

        // сбрасываем цвета всех кнопок
        private void ResetAnswerColors()
        {
            // фон становится фиолетовым 
            Answer0Color = Colors.Purple;
            Answer1Color = Colors.Purple;
            Answer2Color = Colors.Purple;
            Answer3Color = Colors.Purple;

            // текст становится белым
            Answer0TextColor = Colors.White;
            Answer1TextColor = Colors.White;
            Answer2TextColor = Colors.White;
            Answer3TextColor = Colors.White;
        }

        // устанавливаем цвет для кнопки
        private void SetAnswerColor(int index, bool isCorrect)
        {
            // выбираем цвет фона
            Color bgColor;
            if (isCorrect)
            {
                bgColor = Colors.Green; // правильный ответ - зеленый
            }
            else
            {
                bgColor = Colors.Red; // неправильный - красный
            }

            // на цветном фоне текст белый
            Color textColor = Colors.White;

            // применяем цвета к нужной кнопке
            if (index == 0)
            {
                Answer0Color = bgColor;
                Answer0TextColor = textColor;
            }
            else if (index == 1)
            {
                Answer1Color = bgColor;
                Answer1TextColor = textColor;
            }
            else if (index == 2)
            {
                Answer2Color = bgColor;
                Answer2TextColor = textColor;
            }
            else if (index == 3)
            {
                Answer3Color = bgColor;
                Answer3TextColor = textColor;
            }
        }

        // команда при выборе ответа
        [RelayCommand]
        private async Task SelectAnswerAsync(string selectedAnswer)
        {
            // если уже ответили на вопрос - выходим
            if (IsAnswered == true) return;
            if (_answerSelected == true) return;

            // отмечаем что ответ выбран
            _answerSelected = true;

            // проверяем правильный ли ответ
            bool isCorrect = (selectedAnswer == _currentCorrectAnswer);

            // ищем где находится правильный ответ в списке
            int correctIndex = AnswerOptions.IndexOf(_currentCorrectAnswer);

            if (isCorrect == true)
            {
                // правильный ответ записываем 1 в прогресс
                _progress.Answers[_currentQuestionIndex] = 1;
                _progress.NumberOfCorrect = _progress.NumberOfCorrect + 1;

                // подсвечиваем выбранный ответ зеленым
                int selectedIndex = AnswerOptions.IndexOf(selectedAnswer);
                SetAnswerColor(selectedIndex, true);
            }
            else
            {
                // неправильный ответ записываем 0 в прогресс
                _progress.Answers[_currentQuestionIndex] = 0;

                // подсвечиваем выбранный ответ красным
                int selectedIndex = AnswerOptions.IndexOf(selectedAnswer);
                SetAnswerColor(selectedIndex, false);

                // подсвечиваем правильный ответ зеленым
                if (correctIndex >= 0)
                {
                    SetAnswerColor(correctIndex, true);
                }
            }

            // увеличиваем счетчик отвеченных вопросов
            _progress.NumberOfAll = _progress.NumberOfAll + 1;

            // сохраняем прогресс в файл
            await TestingTopicsProgressLoader.UpdateProgressAsync(_topicId, _progress);

            // показываем объяснение
            TestQuestion question = _currentTopic.Questions[_currentQuestionIndex];
            AnswerExplanation = question.AnswerText;

            // отмечаем что вопрос отвечен
            IsAnswered = true;
        }

        // переход к следующему вопросу
        [RelayCommand]
        private async Task NextQuestionAsync()
        {
            // если не ответили - не пускаем дальше
            if (IsAnswered == false) return;

            // проверяем был ли это последний вопрос
            if (_currentQuestionIndex + 1 == _currentTopic.Questions.Count)
            {
                // последний вопрос сохраняем прогресс и идем на результат
                await TestingTopicsProgressLoader.UpdateProgressAsync(_topicId, _progress);
                await Shell.Current.GoToAsync($"///TestingResultPage?topicId={_topicId}");
            }
            else
            {
                // не последний переходим к следующему вопросу
                _currentQuestionIndex = _currentQuestionIndex + 1;
                LoadCurrentQuestion();
            }
        }

        // возврат в меню тестов
        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("///TestingPage");
        }
    }
}