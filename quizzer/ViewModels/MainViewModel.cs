using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using quizzer.Models;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.IO;
namespace quizzer.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _currentQuestionIndex;

    [ObservableProperty]
    private bool _submitEnabled;

    [ObservableProperty]
    private Question? currentQuestion;

    [ObservableProperty]
    private ObservableCollection<Question> _questions = [];

    [ObservableProperty]
    private string _resultText = "";

    [ObservableProperty]
    private int _numberOfQuestions = 0;

    [ObservableProperty]
    private int _asweredQuestions = 0;

    public MainViewModel()
    {
        LoadQuestions();
        if (Questions.Any())
        {
            CurrentQuestion = Questions.First();
            NumberOfQuestions = Questions.Count;
        }
    }

    private void LoadQuestions()
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "questions.json");

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var loadedQuestions = JsonSerializer.Deserialize<List<Question>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (loadedQuestions != null && loadedQuestions.Count > 0)
                {
                    // Zamiešanie otázok a výber prvých 10
                    var randomQuestions = loadedQuestions.OrderBy(q => Guid.NewGuid()).Take(10).ToList();

                    foreach (var question in randomQuestions)
                    {
                        Questions.Add(question);
                    }
                }
                else
                {
                    Console.WriteLine("Error: `questions.json` is empty or invalid.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading questions: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Error: `questions.json` not found at {filePath}");
        }

        CurrentQuestion = Questions.FirstOrDefault();
    }

    [RelayCommand]
    private void NextQuestion()
    {
        if (_currentQuestionIndex < Questions.Count - 1)
        {
            _currentQuestionIndex++;
            CurrentQuestion = Questions[_currentQuestionIndex];
        }
    }


    public void CountAnsweredQuestions(Answer answer)
    {
        answer.AnswerIncorrect = false;
        answer.AnswerCorrect = false;
        AsweredQuestions = Questions.Count(q => q.Answers.Any(a => a.IsSelected));
        SubmitEnabled = AsweredQuestions == Questions.Count;
    }

    [RelayCommand]
    private void PreviousQuestion()
    {
        if (_currentQuestionIndex > 0)
        {
            _currentQuestionIndex--;
            CurrentQuestion = Questions[_currentQuestionIndex];
        }
    }

    [RelayCommand]
    private void RestartTest()
    {
        Questions.Clear();
        LoadQuestions();
        ResultText = "";
        _currentQuestionIndex = 0;
        AsweredQuestions = 0;
        NumberOfQuestions = Questions.Count;
        SubmitEnabled = false;
        CurrentQuestion = Questions.First();
    }

    [RelayCommand]
    private void ValidateTest()
    {
        foreach (var question in Questions)
        {
            foreach (var answer in question.Answers)
            {
                if (answer.IsSelected) // Update only selected answers
                {
                    
                    answer.AnswerCorrect = answer.IsCorrect;
                    answer.AnswerIncorrect = !answer.IsCorrect;
                }

                if(answer.AnswerIncorrect && !answer.IsSelected)
                {
                    answer.AnswerIncorrect = false;
                }

                if (answer.IsCorrect && !answer.IsSelected)
                {
                    answer.AnswerIncorrect = true;
                }
            }
        }

        int correctAnswers = Questions.Count(q => !q.Answers.Any(a => a.AnswerIncorrect));
        int totalQuestions = Questions.Count;

        ResultText = $"You got {correctAnswers} out of {totalQuestions} correct!";
    }
}
