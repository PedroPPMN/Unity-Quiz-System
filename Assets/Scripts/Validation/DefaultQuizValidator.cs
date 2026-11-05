using System;
using System.Linq;
using Portfolio.Quiz.Models;

namespace Portfolio.Quiz.Validation
{
    public sealed class DefaultQuizValidator : IQuizValidator
    {
        public bool IsCorrect(QuestionDefinition question, string[] submittedAnswers)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));
            submittedAnswers ??= Array.Empty<string>();

            return question.Type switch
            {
                QuestionType.TrueFalse => MatchSingle(question, submittedAnswers),
                QuestionType.FillInTheBlank => MatchFill(question, submittedAnswers),
                QuestionType.DragAndDrop => MatchDrag(question, submittedAnswers),
                _ => false
            };
        }

        private static bool MatchSingle(QuestionDefinition question, string[] submitted)
        {
            if (submitted.Length == 0 || question.CorrectAnswers == null || question.CorrectAnswers.Length == 0)
            {
                return false;
            }

            return string.Equals(Normalize(submitted[0]), Normalize(question.CorrectAnswers[0]), StringComparison.OrdinalIgnoreCase);
        }

        private static bool MatchFill(QuestionDefinition question, string[] submitted)
        {
            if (question.CorrectAnswers == null) return false;
            if (submitted.Length != question.CorrectAnswers.Length) return false;

            for (var i = 0; i < submitted.Length; i++)
            {
                if (!string.Equals(Normalize(submitted[i]), Normalize(question.CorrectAnswers[i]), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool MatchDrag(QuestionDefinition question, string[] submitted)
        {
            // submitted format: "source=>target"
            if (question.DragPairs == null) return false;
            var expected = question.DragPairs
                .Select(p => Normalize($"{p.Source}=>{p.Target}"))
                .OrderBy(x => x)
                .ToArray();
            var actual = submitted.Select(Normalize).OrderBy(x => x).ToArray();
            return expected.SequenceEqual(actual);
        }

        private static string Normalize(string value) => (value ?? string.Empty).Trim();
    }
}