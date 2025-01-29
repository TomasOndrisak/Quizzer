using System.Collections.Generic;

namespace quizzer.Models;
public class Question
{
    public string Text { get; set; } = "";
    public List<Answer> Answers { get; set; } = [];
}
