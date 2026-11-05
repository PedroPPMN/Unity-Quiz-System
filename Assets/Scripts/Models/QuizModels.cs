using System;
using System.Collections.Generic;

namespace Portfolio.Quiz.Models
{
    public enum QuestionType
    {
        TrueFalse = 0,
        FillInTheBlank = 1,
        DragAndDrop = 2
    }

    [Serializable]
    public sealed class QuizDefinition
    {
        public string Id;
        public string Title;
        public QuestionDefinition[] Questions;
    }

    [Serializable]
    public sealed class QuestionDefinition
    {
        public string Id;
        public QuestionType Type;
        public string Prompt;
        public string[] Options;
        public string[] CorrectAnswers;
        public DragPair[] DragPairs;
    }

    [Serializable]
    public sealed class DragPair
    {
        public string Source;
        public string Target;
    }

    [Serializable]
    public sealed class QuizListWrapper
    {
        public QuizDefinition[] Quizzes;
    }
}