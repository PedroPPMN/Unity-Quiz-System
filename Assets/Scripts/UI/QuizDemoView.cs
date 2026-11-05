using System.IO;
using Portfolio.Quiz.Controllers;
using Portfolio.Quiz.Models;
using Portfolio.Quiz.Validation;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio.Quiz.UI
{
    public sealed class QuizDemoView : MonoBehaviour
    {
        [SerializeField] private Text promptText;
        [SerializeField] private Text scoreText;
        [SerializeField] private InputField answerInput;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        [SerializeField] private string quizFileName = "sample_quizzes.json";

        private QuizFlowController _flow;

        private void Start()
        {
            _flow = new QuizFlowController(new DefaultQuizValidator());
            _flow.ScoreChanged += RefreshScore;
            _flow.Pagination.IndexChanged += _ => RefreshQuestion();

            var path = Path.Combine(Application.streamingAssetsPath, "Quizzes", quizFileName);
            var json = File.ReadAllText(path);
            var list = JsonUtility.FromJson<QuizListWrapper>(json);
            _flow.Load(list.Quizzes[0]);

            if (submitButton != null) submitButton.onClick.AddListener(OnSubmit);
            if (nextButton != null) nextButton.onClick.AddListener(() => _flow.Pagination.Next());
            if (previousButton != null) previousButton.onClick.AddListener(() => _flow.Pagination.Previous());

            RefreshQuestion();
            RefreshScore();
        }

        private void OnSubmit()
        {
            var answer = answerInput != null ? answerInput.text : string.Empty;
            var ok = _flow.SubmitCurrent(answer);
            if (promptText != null)
            {
                promptText.text = $"{_flow.CurrentQuestion.Prompt}\n\n{(ok ? "Correct" : "Try again")}";
            }
        }

        private void RefreshQuestion()
        {
            var q = _flow.CurrentQuestion;
            if (q == null || promptText == null) return;
            promptText.text = $"[{q.Type}] {q.Prompt}";
            if (answerInput != null) answerInput.text = string.Empty;
        }

        private void RefreshScore()
        {
            if (scoreText == null) return;
            scoreText.text = $"Score: {_flow.CorrectCount}/{_flow.AnsweredCount}";
        }
    }
}