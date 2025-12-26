using lapp3.Data;
using lapp3.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lapp3.Services
{
    public class CourseService

    {

        public static void ShowActiveCourses()
        {
            using var context = new Data.NykopingsgymnasiumContext();

            var activeCourses= context.Courses
                .Where(c => c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToList();

            var table = new Table();
            table.AddColumn("Course ID");
            table.AddColumn("Course Name");

            foreach (var course in activeCourses)
            {
                table.AddRow(course.CourseId.ToString(), course.CourseName);
            }

            AnsiConsole.Write(table);

            Console.ReadKey();






        }



        public static void ShowInactiveCourses() 
        {
            using var context = new Data.NykopingsgymnasiumContext();
            var inactiveCourses = context.Courses
                .Where(c => !c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToList();


            var table = new Table();
            table.AddColumn("Course ID");
            table.AddColumn("Course Name");
            foreach (var course in inactiveCourses)
            {
                table.AddRow(course.CourseId.ToString(), course.CourseName);

            }

            AnsiConsole.Write(table);
            Console.ReadKey();

        }



        public static void AddCourse()
        {
            using var context = new Data.NykopingsgymnasiumContext();
            string courseName = AnsiConsole.Ask<string>("Enter the [green]course name[/]: ");
            bool isActive = AnsiConsole.Confirm("Is the course active?");
            var newCourse = new Models.Course
            {
                CourseName = courseName,
                IsActive = isActive
            };
            context.Courses.Add(newCourse);
            context.SaveChanges();
            AnsiConsole.MarkupLine("[green]Course added successfully![/]");
            Console.ReadKey();
        }

        public static void DeleteCourse()
        {
             using var context = new Data.NykopingsgymnasiumContext();

            var courses = context.Courses
                .OrderBy(c => c.CourseName)
                .ToList();


            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Course>()
                    .Title("Välj kurs att ta bort:")
                    .UseConverter(c => $"{c.CourseId} - {c.CourseName}")
                    .AddChoices(courses));


            var confirm = AnsiConsole.Confirm($"Är du säker på att du vill ta bort kursen [red]{selected.CourseName}[/]?");
            if (confirm)
            {
                context.Courses.Remove(selected);
                context.SaveChanges();
                AnsiConsole.MarkupLine("[green]Kursen har tagits bort.[/]");
            }

            else
            {
                AnsiConsole.MarkupLine("[yellow]Borttagningen avbröts.[/]");
            }

            Console.ReadKey();

            return;


            

          

        }


        public static void ActivateCourse()
        {

            using var context = new NykopingsgymnasiumContext();

            var inactiveCourses = context.Courses
                .Where(c => !c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToList();



            if (!inactiveCourses.Any())
            {
                AnsiConsole.MarkupLine("[green]Alla kurser är redan aktiva.[/]");
                Console.ReadKey();
                return;
            }


            var selectedCourse = AnsiConsole.Prompt(
                new SelectionPrompt<Course>()
                    .Title("Välj kurs att aktivera:")
                    .UseConverter(c => $"{c.CourseId} - {c.CourseName}")
                    .AddChoices(inactiveCourses)
                    );

            var confirm = AnsiConsole.Confirm(
                $"Vill du aktivera kursen [green]{selectedCourse.CourseName}[/]?" );






            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Åtgärden avbröts.[/]");
                Console.ReadKey();
                return;
            }


            selectedCourse.IsActive = true;
            context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Kursen har aktiverats![/]");
            Console.ReadKey();
        }


        public static void DeactivateCourse()
        {
            using var context = new NykopingsgymnasiumContext();
            var activeCourses = context.Courses
                .Where(c => c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToList();
            if (!activeCourses.Any())
                {
                AnsiConsole.MarkupLine("[green]Alla kurser är redan inaktiva.[/]");
                Console.ReadKey();
                return;
            }
            var selectedCourse = AnsiConsole.Prompt(
                new SelectionPrompt<Course>()
                    .Title("Välj kurs att inaktivera:")
                    .UseConverter(c => $"{c.CourseId} - {c.CourseName}")
                    .AddChoices(activeCourses)
                    );
            var confirm = AnsiConsole.Confirm(
                $"Vill du inaktivera kursen [red]{selectedCourse.CourseName}[/]?"
                );
            if (!confirm)
                {
                AnsiConsole.MarkupLine("[yellow]Åtgärden avbröts.[/]");
                Console.ReadKey();
                return;
            }
           
            selectedCourse.IsActive = false;
            context.SaveChanges();
            AnsiConsole.MarkupLine("[green]Kursen har inaktiverats![/]");
            Console.ReadKey();










        }

}
}






