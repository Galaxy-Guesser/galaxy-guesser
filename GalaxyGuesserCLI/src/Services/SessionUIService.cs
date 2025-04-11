using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Spectre.Console;
public static class SessionUIService
{
    public static string PromptCategory()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a session category:")
                .AddChoices("Science", "History", "Geography", "Space Exploration"));
    }

    public static int PromptQuestionCount()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the number of questions:")
                .Validate(val => val > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Must be a positive number[/]"))
                .DefaultValue(10));
    }

     public static (string category, int questionCount) PromptSessionDetails()
    {
        var category = PromptCategory();
        var questionCount = PromptQuestionCount();
        return (category, questionCount);
    }
}
