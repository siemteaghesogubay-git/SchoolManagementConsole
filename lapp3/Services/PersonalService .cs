using lapp3.Data;
using lapp3.Models;
using Spectre.Console;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace lapp3.Services
{
    
   
        public static class PersonalService

    {
        // Hämtar Personal 

        public static void Showpersonal()
        {

            using var context = new NykopingsgymnasiumContext();

            Spectre.Console.AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Välj en personal att visa information om:")

                    .AddChoices(
                    "Alla",
                    "Teacher",
                    "Administrator",
                    "IT Support",
                    "Principal"
                     )

                    

            );


            var personals = choice == "Alla"
                ? context.Personals.ToList()
                : context.Personals
                    .Where(p => p.Position == choice)
                    .ToList();


            // lärde mig detta från ChatGPT när jag frågade hur kan jag förbättra koden..

            if (!personals.Any())
            {
                AnsiConsole.MarkupLine("[red]Ingen personal hittades.[/]");
                
                return;
            }




            var tableP = new Spectre.Console.Table()
               .Border(TableBorder.Rounded)
                     .AddColumn("Förnamn")
                     .AddColumn("Efternamn")
                     .AddColumn("role /befattning ");




            
            foreach (var personal in personals)
            {
                tableP.AddRow(
                    personal.FristName ,
                    personal.LastName ,
                    personal.Position 
                );
            }



             
            

            AnsiConsole.Write(
                new Rule("[bold blue]Personal[/]")
                    .RuleStyle("grey")
                    .Centered());

            AnsiConsole.Write(tableP);

           
            
            Console.ReadKey();











        }




        public static void AddPersonal()
            {
            using var context = new NykopingsgymnasiumContext();
            Spectre.Console.AnsiConsole.Clear();
            var firstName = AnsiConsole.Ask<string>("Ange förnamn:");
            var lastName = AnsiConsole.Ask<string>("Ange efternamn:");
            var socialSecurityNumber = AnsiConsole.Ask<string>("Ange personnummer:");
            var position = AnsiConsole.Ask<string>("Ange befattning:");
            var newPersonal = new Models.Personal
            {
                FristName = firstName,
                LastName = lastName,
                SocialSecurityNumber = socialSecurityNumber,
                Position = position
            };
            context.Personals.Add(newPersonal);
            context.SaveChanges();
            AnsiConsole.MarkupLine("[green]Ny personal har lagts till framgångsrikt![/]");











        }





        public static void DeletePersonal()
        {
            using var context = new NykopingsgymnasiumContext();

            // Hämta all personal
            var personals = context.Personals.ToList();

            if (!personals.Any())
            {
                AnsiConsole.MarkupLine("[red]Ingen personal att ta bort.[/]");
                Console.ReadKey();
                return;
            }

            // Låt användaren välja personal
            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Personal>()
                    .Title("Välj personal att ta bort:")
                    .UseConverter(p => $"{p.PersonalId}: {p.FristName} {p.LastName} – {p.Position}")
                    .AddChoices(personals)
            );

            // Bekräfta borttagning
            var confirm = AnsiConsole.Confirm($"Är du säker på att du vill ta bort [red]{selected.FristName} {selected.LastName}[/]?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Borttagningen avbröts.[/]");
                Console.ReadKey();
                return;
            }

            // Ta bort personal
            context.Personals.Remove(selected);
            context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Personal borttagen![/]");
            Console.ReadKey();
        }


        public static void ShowPersonalByRole()
        {
            using var context = new NykopingsgymnasiumContext();
            AnsiConsole.Clear();

            var result = context.Personals
                .GroupBy(p => p.Position)
                .Select(g => new
                {
                    Position = g.Key,
                    Count = g.Count()
                })
                .ToList();

            if (!result.Any())
            {
                AnsiConsole.MarkupLine("[red]Ingen personal hittades.[/]");
                Console.ReadKey();
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("Befattning")
                .AddColumn("Antal personal");

            foreach (var item in result)
            {
                table.AddRow(
                    item.Position ?? "Okänd",
                    item.Count.ToString()
                );
            }

            AnsiConsole.Write(
                new Rule("[bold blue]Personal per befattning[/]")
                    .RuleStyle("grey")
                    .Centered());

            AnsiConsole.Write(table);
            Console.ReadKey();
        }






    }








}




