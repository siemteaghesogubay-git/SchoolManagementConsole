using System;
using Spectre.Console;
using lapp3.Services;

namespace lapp3.Services.Menu
{
    public static class Menu
    {
        // ============================
        // STARTMENY
        // ============================
        public static void ShowStartMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold green]Välj huvudmeny:[/]")
                        .AddChoices("Personal", "Studenter", "Avsluta")
                );

                switch (choice)
                {
                    case "Personal":
                        ShowPersonalMenu();
                        break;

                    case "Studenter":
                        ShowStudentMenu();
                        break;

                    case "Avsluta":
                        AnsiConsole.MarkupLine("[yellow]Programmet avslutas...[/]");
                        return; // Avslutar programmet
                }
            }
        }

        // ============================
        // PERSONALMENY
        // ============================
        private static void ShowPersonalMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold blue]Personal - Välj funktion:[/]")
                        .AddChoices(
                            "Visa all personal",
                            "Lägg till personal",
                            "Ta bort personal",
                            "Tillbaka"
                        )
                );

                switch (choice)
                {
                    case "Visa all personal":
                        PersonalService.Showpersonal();
                        break;

                    case "Lägg till personal":
                        PersonalService.AddPersonal();
                        break;

                    case "Ta bort personal":
                        PersonalService.DeletePersonal();
                        break;

                    case "Tillbaka":
                        return; // Går tillbaka till startmenyn
                }
            }
        }

        // ============================
        // STUDENTMENY
        // ============================
        public static void ShowStudentMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold blue]Studenter - Välj funktion:[/]")
                        .AddChoices(
                            "Visa alla studenter",
                            "Visa studenter via klass",
                            "Lägg till student",
                            "Tillbaka"
                        )
                );

                switch (choice)
                {
                    case "Visa alla studenter":
                        StudentService.ShowAllStudents();
                        break;

                    case "Visa studenter via klass":
                        StudentService.ShowStudentsByClass();
                        break;

                    case "Lägg till student":
                        StudentService.AddStudent();
                        break;

                    case "Tillbaka":
                        return; // Går tillbaka till startmenyn
                }
            }
        }
    }
}
