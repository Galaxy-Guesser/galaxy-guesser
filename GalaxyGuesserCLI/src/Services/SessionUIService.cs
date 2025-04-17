using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyGuesserCli.Models;
using GalaxyGuesserCli.Services;

using Spectre.Console;

public static class SessionUIService
{

    public static async Task<(string category, int categoryId)> PromptCategory()
    {
        var categories = await CategoryService.GetCategoriesAsync();

        var selectedName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a session category:")
                .AddChoices(categories.Select(c => c.category).ToList()));

        var selectedCategory = categories.First(c => c.category == selectedName);
        return (selectedCategory.category, selectedCategory.categoryId);
    }


    public static int PromptQuestionCount(int availableQuestionCount)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the number of questions:")
                .Validate(val =>
                {
                    if (val <= 0)
                        return ValidationResult.Error("[red]Must be a positive number[/]");
                    if (val > availableQuestionCount)
                        return ValidationResult.Error($"[red]This category only has {availableQuestionCount} questions available[/]");
                    return ValidationResult.Success();
                })
                .DefaultValue(availableQuestionCount));
    }

    public static double PromptSessionDuration(int questionCount)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<double>($"Enter session duration in hours [grey](e.g., 0.5, 1, 1.5...)[/]:")
                .DefaultValue(1.0)
                .Validate(duration =>
                {
                    if (duration <= 0)
                        return ValidationResult.Error("[red]Duration must be a positive number[/]");

                    if ((duration * 60) % 30 != 0)
                        return ValidationResult.Error("[red]Duration must be in 30-minute intervals (e.g., 0.5, 1, 1.5, ...)[/]");

                    var totalAvailableSeconds = duration * 3600;
                    var requiredSeconds = questionCount * 15;

                    if (requiredSeconds > totalAvailableSeconds)
                        return ValidationResult.Error($"[red]Duration too short. You need at least {Math.Ceiling(requiredSeconds / 60.0)} minutes for {questionCount} questions.[/]");

                    return ValidationResult.Success();
                }));
    }


    public static DateTime PromptStartDateTime()
    {
        var now = DateTime.Now;
        int roundedMinute = now.Minute - (now.Minute % 5);
        string defaultDate = now.ToString("yyyy-MM-dd");
        string defaultTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, roundedMinute, 0)
            .ToString("HH:mm");

        string dateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter session date [grey](YYYY-MM-DD)[/]:")
                .DefaultValue(defaultDate)
                .Validate(input =>
                    DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Invalid date format. Use YYYY-MM-DD[/]"))
        );

        string timeInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter session time [grey](HH:MM, 24-hour format)[/]:")
                .Validate(input =>
                {
                    var parts = input.Split(':');
                    if (parts.Length != 2 ||
                        !int.TryParse(parts[0], out int h) ||
                        !int.TryParse(parts[1], out int m))
                        return ValidationResult.Error("[red]Invalid format. Use HH:MM[/]");

                    if (h < 0 || h > 23 || m < 0 || m > 59)
                        return ValidationResult.Error("[red]Invalid time value[/]");


                    var date = DateTime.ParseExact(dateInput, "yyyy-MM-dd", null);
                    var result = new DateTime(date.Year, date.Month, date.Day, h, m, 0);

                    if (result <= DateTime.Now)
                        return ValidationResult.Error("[red]Start time must be in the future[/]");

                    return ValidationResult.Success();
                })
        );

        var (hour, minute) = timeInput.Split(':') switch
        {
            [var h, var m] => (int.Parse(h), int.Parse(m)),
            _ => throw new FormatException("Unexpected time format")
        };

        var date = DateTime.ParseExact(dateInput, "yyyy-MM-dd", null);
        return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
    }

    public static async Task<(string category, int questionCount, string startTime, decimal sessionDuration)> PromptSessionDetails()
    {
        var (category, categoryId) = await PromptCategory();
        int availableQuestionCount = await QuestionsService.FetchSessionQuestionsCountAsync(categoryId);
        if (availableQuestionCount == 0)
        {
            Console.WriteLine("[red]No available questions for this category. Please try a different category.[/]");
        }
        var questionCount = PromptQuestionCount(availableQuestionCount); ;
        decimal sessionDuration = (decimal)PromptSessionDuration(questionCount);
        var startTime = PromptStartDateTime();

        string formattedStartTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss");

        return (category, questionCount, formattedStartTime, sessionDuration);
    }

}
