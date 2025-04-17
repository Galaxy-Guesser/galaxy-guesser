using GalaxyGuesserCli.Models;
using GalaxyGuesserCli.Services;
using GalaxyGuesserCli.Utilities;
using Spectre.Console;
using GalaxyGuesserCLI.Services;

namespace GalaxyGuesserCli
{
    class GalaxyQuiz
    {
        public static async Task Start()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "🌌 Galaxy Quiz";
            Console.CursorVisible = false;

            try
            {
                var httpClient = new HttpClient();
                var nasaService = new NasaService(httpClient);

                UIService.PrintGalaxyHeader();
                var authService = new AuthenticationService();
                var jwt = await authService.AuthenticateWithGoogle();

                var player = await authService.AuthOrRegisterWithBackend();

                var fact = await nasaService.GetSpaceFactAsync();

                UIService.DisplaySpaceFact(fact);

                await MainMenuLoop(player);
            }
            catch (Exception exception)
            {
                ErrorHandler.HandleError(exception);
            }
        }

        static async Task MainMenuLoop(Player player)
        {

            bool exitRequested = false;

            while (!exitRequested)
            {
                AnsiConsole.Clear();

                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine($"\n👋 Welcome, [bold]{player.userName}[/]!\n");

                var menuActions = new Dictionary<string, Func<Task>>
                {
                    ["Create new quiz session"] = async () => {
                    var (category, questionCount, startTime,sessionDuration) = await SessionUIService.PromptSessionDetails();
                    AnsiConsole.MarkupLine($"Category: [cyan]{category}[/], Question Count: [cyan]{questionCount}[/], Start Time: [cyan]{startTime}[/]");

                    string sessionCode = await SessionService.CreateSessionAsync(category, questionCount,startTime, sessionDuration);

                    if (!string.IsNullOrEmpty(sessionCode))
                        AnsiConsole.MarkupLine($"✅ Session created! Code: [bold yellow]{sessionCode}[/]");
                    else
                        AnsiConsole.MarkupLine("[red]❌ Failed to create session.[/]");
                    },

                    ["Join existing session"] = async () => {
                        var activeSessions = await SessionViewService.GetActiveSessions();

                        if (activeSessions.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[red]No active sessions available.[/]");
                            return;
                        }

                        var selectedSession = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Select a session to join")
                        .PageSize(10)
                        .AddChoices(activeSessions.Select(s => $"{s.sessionCode} - {s.category}")));

                        string sessionCode = selectedSession.Split(" - ")[0];
                        await SessionService.JoinSessionAsync(sessionCode);
                    },

                    ["View active sessions"] = async () => {
                        var sessions = await SessionViewService.GetActiveSessions();
                        await UIService.DisplayActiveSessionsAsync(sessions);
                    },

                    ["View leaderboard"] = () => {
                        CommandService.ProcessCommand("leaderboard", player);
                        return Task.CompletedTask;
                    },

                    ["My profile"] = () => {
                        CommandService.ProcessCommand("myprofile", player);
                        return Task.CompletedTask;
                    },

                    ["How to play"] = () => {
                        UIService.ShowHowToPlay();
                        return Task.CompletedTask;
                    },

                    ["Exit"] = () => {
                        if (AnsiConsole.Confirm("Are you sure you want to exit?"))
                        {
                            exitRequested = true;
                            AnsiConsole.MarkupLine("\n[green]👋 Thanks for playing Galaxy Quiz! See you among the stars![/]");
                            Thread.Sleep(2000);
                        } else {
                            //do nothing
                        }
                        return Task.CompletedTask;
                    }
                };

                var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(menuActions.Keys));

                await menuActions[selection]();

                if (!exitRequested)
                UIService.Continue();
            }
        }
    }
}