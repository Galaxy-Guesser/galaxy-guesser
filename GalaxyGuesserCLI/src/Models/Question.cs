 namespace GalaxyGuesserCli.Models
{


class Question
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Text { get; set; }
    public string[] Options { get; set; }
    public int CorrectAnswerIndex { get; set; }

    public Question(int id, int categoryId, string text, string[] options, int correctAnswerIndex)
    {
        Id = id;
        CategoryId = categoryId;
        Text = text;
        Options = options;
        CorrectAnswerIndex = correctAnswerIndex;
    }
}
}