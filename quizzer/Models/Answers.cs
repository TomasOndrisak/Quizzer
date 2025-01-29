using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace quizzer.Models;
public partial class Answer : ObservableObject
{
    public string Text { get; set; } = "";
    public bool IsCorrect { get; set; }
    [ObservableProperty]
    [JsonIgnore]
    public bool _answerIncorrect;
    [ObservableProperty]
    [JsonIgnore]
    public bool _answerCorrect;
    [ObservableProperty]
    [JsonIgnore]
    public bool _isSelected;
}

