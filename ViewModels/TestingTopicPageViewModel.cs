using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NooBasket.Models;
using Microsoft.Maui.Graphics;
using NooBasket.Services;

namespace NooBasket.ViewModels
{
    // позволяет получить номер темы из строки навигации
    [QueryProperty(nameof(TopicId), "topicId")]
    [QueryProperty(nameof(ReturnRoute), "returnRoute")]
    [QueryProperty(nameof(ReturnToResults), "returnToResults")]
    [QueryProperty(nameof(PrevCorrect), "prevCorrect")]
    [QueryProperty(nameof(PrevTotal), "prevTotal")]
    public partial class TestingTopicPageViewModel : ObservableObject
    {
        private int _prevCorrect;
        private int _prevTotal;

        public int PrevCorrect
        {
            get => _prevCorrect;
            set => _prevCorrect = value;
        }

        public int PrevTotal
        {
            get => _prevTotal;
            set => _prevTotal = value;
        }
        [ObservableProperty]
        private string _returnToResults = "";
        [ObservableProperty]
        private string _returnRoute = "";
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
        private string _nextButtonText = "Следующий вопрос";

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
        private Color _answer0Color = Color.FromArgb("#D2691E");

        // цвет фона второй кнопки
        [ObservableProperty]
        private Color _answer1Color = Color.FromArgb("#D2691E");

        // цвет фона третьей кнопки
        [ObservableProperty]
        private Color _answer2Color = Color.FromArgb("#D2691E");

        // цвет фона четвертой кнопки
        [ObservableProperty]
        private Color _answer3Color = Color.FromArgb("#D2691E");

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

        // временный прогресс по теме (не сохраняется в файл до завершения)
        private TestingTopicsProgress? _tmpProgress;

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

                // создаем временный прогресс для этого теста (начинаем с нуля)
                _tmpProgress = new TestingTopicsProgress();
                _tmpProgress.Answers = new List<int>(new int[_currentTopic.Questions.Count]);
                _tmpProgress.NumberOfCorrect = 0;
                _tmpProgress.NumberOfAll = 0;

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
                NextButtonText = "Завершить тест";
            }
            else
            {
                NextButtonText = "Следующий вопрос";
            }
        }

        // сбрасываем цвета всех кнопок
        private void ResetAnswerColors()
        {
            Answer0Color = Color.FromArgb("#E28F04");
            Answer1Color = Color.FromArgb("#E28F04");
            Answer2Color = Color.FromArgb("#E28F04");
            Answer3Color = Color.FromArgb("#E28F04");

            Answer0TextColor = Color.FromArgb("#FFE9C4");
            Answer1TextColor = Color.FromArgb("#FFE9C4");
            Answer2TextColor = Color.FromArgb("#FFE9C4");
            Answer3TextColor = Color.FromArgb("#FFE9C4");
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

            // берем вопрос чтобы потом взять из него пояснение
            TestQuestion question = _currentTopic.Questions[_currentQuestionIndex];
            string baseExplanation = question.AnswerText;

            // переменная для хранения сообщения о результате
            string resultMessage;

            if (isCorrect == true)
            {
                // правильный ответ - сохраняем 1 в прогресс
                _tmpProgress.Answers[_currentQuestionIndex] = 1;
                _tmpProgress.NumberOfCorrect = _tmpProgress.NumberOfCorrect + 1;

                // подсвечиваем выбранный ответ зеленым
                int selectedIndex = AnswerOptions.IndexOf(selectedAnswer);
                SetAnswerColor(selectedIndex, true);

                // сообщение что правильно
                resultMessage = "Правильно!";
            }
            else
            {
                // неправильный ответ - сохраняем 0 в прогресс
                _tmpProgress.Answers[_currentQuestionIndex] = 0;

                // подсвечиваем выбранный ответ красным
                int selectedIndex = AnswerOptions.IndexOf(selectedAnswer);
                SetAnswerColor(selectedIndex, false);

                // подсвечиваем правильный ответ зеленым
                if (correctIndex >= 0)
                {
                    SetAnswerColor(correctIndex, true);
                }

                // сообщение что неправильно
                resultMessage = "Неправильно!";
            }

            // склеиваем результат и пояснение
            AnswerExplanation = resultMessage + "\n\nПояснение: " + baseExplanation;

            // увеличиваем счетчик отвеченных вопросов
            _tmpProgress.NumberOfAll = _tmpProgress.NumberOfAll + 1;

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
                // сохраняем прогресс в файл (там внутри уже логика что сохраняется только лучший)
                await TestingTopicsProgressLoader.UpdateProgressAsync(_topicId, _tmpProgress);

                // передаем на страницу результатов не только id темы, но и результаты этой попытки
                // для этого используем словарь с параметрами
                Dictionary<string, object> navigationParams = new Dictionary<string, object>
                {
                    { "topicId", _topicId },
                    { "lastAttemptCorrect", _tmpProgress.NumberOfCorrect },
                    { "lastAttemptTotal", _tmpProgress.NumberOfAll }
                };

                await Shell.Current.GoToAsync($"///TestingResultPage", navigationParams);
            }
            else
            {
                // не последний - переходим к следующему вопросу
                _currentQuestionIndex = _currentQuestionIndex + 1;
                LoadCurrentQuestion();
            }
        }
        [RelayCommand]
        private async Task GoBackAsync()
        {
            bool answer = await Shell.Current.DisplayAlert(
                 "Выйти из теста? ",
                 "Если вы выйдете из теста сейчас, текущий прогресс не сохранится. ",
                 "Выйти ",
                 "Продолжить тестирование "
            );

            if (answer)
            {
                if (ReturnToResults == "yes")
                {
                    var navigationParams = new Dictionary<string, object>
            {
                { "topicId", _topicId },
                { "lastAttemptCorrect", _prevCorrect },
                { "lastAttemptTotal", _prevTotal }
            };

                    await Shell.Current.GoToAsync("//TestingResultPage", navigationParams);
                }
                else if (ReturnRoute == "EducationTopicPage")
                {
                    await Shell.Current.GoToAsync($"//EducationTopicPage?topicId={TopicId}");
                }
                else
                {
                    await Shell.Current.GoToAsync("//TestingPage");
                }
            }
        }
    }
}