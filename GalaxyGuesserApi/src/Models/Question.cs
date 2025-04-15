
using System.Collections.Generic;

namespace GalaxyGuesserApi.Models
{
  public class Question
  {
    public int questionId { get; set; }
    public string question { get; set; }
    public int answerId { get; set; }
    public int categoryId { get; set; }
    public string text { get; set; }
    public List<Option> Options { get; set; }
  }
}

