using System;
using System.Collections.Generic;
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

    public static int PromptQuestionDuration()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("Enter duration per question (in seconds):")
                .DefaultValue(30)
                .Validate(val =>
                {
                    if (val <= 0)
                        return ValidationResult.Error("[red]Duration must be a positive number[/]");
                    if (val > 60)
                        return ValidationResult.Error("[red]Duration must not exceed 60 seconds[/]");
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
                .DefaultValue(defaultTime)
                .Validate(input =>
                {
                    var parts = input.Split(':');
                    if (parts.Length != 2 ||
                        !int.TryParse(parts[0], out int h) ||
                        !int.TryParse(parts[1], out int m))
                        return ValidationResult.Error("[red]Invalid format. Use HH:MM[/]");

                    if (h < 0 || h > 23 || m < 0 || m > 59)
                        return ValidationResult.Error("[red]Invalid time value[/]");

                    if (m % 5 != 0)
                        return ValidationResult.Error("[red]Minutes must be in 5-minute intervals[/]");

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


    public static (string category, int questionCount, string startTime, int questionDuration) PromptSessionDetails()
    {
        var category = PromptCategory();
        var questionCount = PromptQuestionCount();
        var questionDuration = PromptQuestionDuration();
        var startTime = PromptStartDateTime();

        string formattedStartTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss");

        return (category, questionCount, formattedStartTime, questionDuration);
    }
}
