using System;
using System.Collections.Generic;
using Portfolio.Quiz.Models;
using Portfolio.Quiz.Validation;

namespace Portfolio.Quiz.Controllers
{
    public sealed class QuizFlowController
    {
        private readonly IQuizValidator _validator;
        private readonly PaginationController _pagination = new();
        private readonly Dictionary<string, string[]> _answers = new();
        private QuizDefinition _quiz;

        public PaginationController Pagination => _pagination;
        public QuizDefinition Quiz => _quiz;
        public QuestionDefinition CurrentQuestion =>
            _quiz == null || _quiz.Questions == null || _quiz.Questions.Length == 0
                ? null
                : _quiz.Questions[_pagination.CurrentIndex];

        public event Action ScoreChanged;
        public int CorrectCount { get; private set; }
        public int AnsweredCount => _answers.Count;

        public QuizFlowController(IQuizValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public void Load(QuizDefinition quiz)
        {
            _quiz = quiz ?? throw new ArgumentNullException(nameof(quiz));
            _answers.Clear();
            CorrectCount = 0;
            _pagination.Reset(quiz.Questions?.Length ?? 0);
            ScoreChanged?.Invoke();
        }

        public bool SubmitCurrent(params string[] answers)
        {
            var question = CurrentQuestion;
            if (question == null) return false;

            _answers[question.Id] = answers ?? Array.Empty<string>();
            Recalculate();
            return _validator.IsCorrect(question, answers);
        }

        private void Recalculate()
        {
            CorrectCount = 0;
            if (_quiz?.Questions == null) return;

            foreach (var question in _quiz.Questions)
            {
                if (!_answers.TryGetValue(question.Id, out var submitted)) continue;
                if (_validator.IsCorrect(question, submitted)) CorrectCount++;
            }

            ScoreChanged?.Invoke();
        }
    }
}