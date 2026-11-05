using Portfolio.Quiz.Models;

namespace Portfolio.Quiz.Validation
{
    public interface IQuizValidator
    {
        bool IsCorrect(QuestionDefinition question, string[] submittedAnswers);
    }
}