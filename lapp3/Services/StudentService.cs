using lapp3.Data;
using lapp3.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace lapp3.Services
{
    public class StudentService


    {

        public static void ShowAllStudents()
        {

            AnsiConsole.Clear();


            // skapar en ny context för att kunna kommunicera med databasen

            using var context = new NykopingsgymnasiumContext();



            // frågar användaren hur de vill sortera eleverna för och efrernamn eller efternamn för och efternamn samt klass 


            var sortOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hur vill du sortera eleverna?")
                    .AddChoices(new[] {
                        "Förnamn Efternamn",
                        "Efternamn Förnamn",
                        "Klass Förnamn Efternamn"
                    }));

            //frågar använaren i vileken ordning de vill sortera eleverna (A till Ö eller Ö till A)

            var sortOrder = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                   .Title("I vilken ordning vill du sortera eleverna?")
                   .AddChoices(new[] {
                        "Stigande",
                        "Fallande"


                   }));


            var students = context.Students

                .Include(s => s.Class).
                AsQueryable();



            bool ascending = sortOrder == "Stigande";


            students = sortOption switch


            {
                "Förnamn Efternamn" => ascending
                ? students.OrderBy(students => students.FristName)
                .ThenBy(students => students.LastName)
                : students.OrderByDescending(students => students.FristName)
                .ThenByDescending(students => students.LastName),





                " Efternamn Förnamn" => ascending
                ? students.OrderBy(students => students.LastName)
                .ThenBy(students => students.FristName)
                : students.OrderByDescending(students => students.LastName)
                .ThenByDescending(students => students.FristName),



                "Klass Förnamn Efternamn" => ascending

                ? students.OrderBy(students => students.Class.ClassName)
                .ThenBy(students => students.FristName)
                .ThenBy(students => students.LastName)

                : students.OrderByDescending(s => s.Class.ClassName)
                      .ThenByDescending(s => s.FristName)
                      .ThenByDescending(s => s.LastName),


                _ => students


            };

            var table = new Spectre.Console.Table()
               .Border(TableBorder.Rounded)
                     .AddColumn("Förnamn")
                     .AddColumn("Efternamn")
                     .AddColumn("Klass");






            foreach (var student in students)
            {
                table.AddRow

                     (student.FristName,
                         student.LastName,
                          student.Class?.ClassName ?? "-"
                     );






            }



            AnsiConsole.Write(
          new Rule("[bold blue]Alla studenter[/]")
          .RuleStyle("grey")
           .Centered()
                       );


            AnsiConsole.Clear();
            AnsiConsole.Write(table);
            Console.ReadKey();

        }






        // Sortera studenter efer klass 

        public static void ShowStudentsByClass()
        {
            using var context = new NykopingsgymnasiumContext();

            AnsiConsole.Clear();
            var classes = context.Classes.ToList();


            var selectedclass = AnsiConsole.Prompt(
                new SelectionPrompt<Class>()
                    .Title("Välj en klass:")
                    .UseConverter(c => c.ClassName)
                    .AddChoices(classes));


            var studentsInClass = context.Students
                .Where(s => s.ClassId == selectedclass.ClassId)
                .OrderBy(s => s.FristName)
                .ThenBy(s => s.LastName)
                .ToList();

            AnsiConsole.Clear();




            foreach (var student in studentsInClass)
            {
                AnsiConsole.MarkupLine($"[green]{student.FristName} {student.LastName}[/]");
            }


            Console.ReadKey();
        }


        public static void AddStudent()
        {
            using var context = new NykopingsgymnasiumContext();
            AnsiConsole.Clear();
            var firstName = AnsiConsole.Ask<string>("Ange elevens [green]förnamn[/]:");
            var lastName = AnsiConsole.Ask<string>("Ange elevens [green]efternamn[/]:");
            var classes = context.Classes.ToList();
            var selectedClass = AnsiConsole.Prompt(
                new SelectionPrompt<Class>()
                    .Title("Välj en klass för eleven:")
                    .UseConverter(c => c.ClassName)
                    .AddChoices(classes));
            var newStudent = new Student
            {
                FristName = firstName,
                LastName = lastName,
                ClassId = selectedClass.ClassId
            };
            context.Students.Add(newStudent);
            context.SaveChanges();
            AnsiConsole.MarkupLine($"[green]Elev {firstName} {lastName} har lagts till i klassen {selectedClass.ClassName}.[/]");
            Console.ReadKey();






        }



        public static void ShowAllStudentsWithDetails()
        {
            using var context = new NykopingsgymnasiumContext();
            AnsiConsole.Clear();

            var students = context.Students
                .Include(s => s.Class)
                .Include(s => s.CourseGrades)
                    .ThenInclude(cg => cg.Course)
                .ToList();

            if (!students.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga studenter hittades.[/]");
                Console.ReadKey();
                return;
            }

            foreach (var student in students)
            {
                AnsiConsole.Write(
                    new Rule($"[bold blue]{student.FristName} {student.LastName}[/]")
                        .RuleStyle("grey")
                        .Centered());

                AnsiConsole.MarkupLine(
                    $"[bold]Klass:[/] {student.Class?.ClassName ?? "Okänd"}");

                if (!student.CourseGrades.Any())
                {
                    AnsiConsole.MarkupLine("[yellow]Inga betyg registrerade.[/]");
                }
                else
                {
                    var table = new Table()
                        .Border(TableBorder.Rounded)
                        .AddColumn("Kurs")
                        .AddColumn("Betyg");

                    foreach (var grade in student.CourseGrades)
                    {
                        table.AddRow(
                            grade.Course?.CourseName ?? "Okänd kurs",
                            grade.Grade ?? "-"
                        );
                    }

                    AnsiConsole.Write(table);
                }

                AnsiConsole.WriteLine(); // luft mellan studenter
            }

            Console.ReadKey();
        }



        public static void SetGradeWithTransaction()
        {
            using var context = new NykopingsgymnasiumContext();
            AnsiConsole.Clear();

            var student = AnsiConsole.Prompt(
                new SelectionPrompt<Student>()
                    .Title("Välj student:")
                    .UseConverter(s => $"{s.StudentId} - {s.FristName} {s.LastName}")
                    .AddChoices(context.Students.ToList())
            );
            var teacher = AnsiConsole.Prompt(
                new SelectionPrompt<Personal>()
                    .Title("Välj lärare:")
                    .UseConverter(t => $"{t.PersonalId} - {t.FristName} {t.LastName}")
                    .AddChoices(
                        context.Personals
                            .Where(p => p.Position == "Teacher")
                            .ToList()
                    )
            );

            using var transaction = context.Database.BeginTransaction();

            try
            {
                while (true)
                {
                    
                    var course = AnsiConsole.Prompt(
                        new SelectionPrompt<Course>()
                            .Title("Välj kurs:")
                            .UseConverter(c => c.CourseName)
                            .AddChoices(context.Courses.ToList())
                    );

                   
                    var gradeValue = AnsiConsole.Ask<string>(
                        "Ange betyg (A–F):"
                    ).ToUpper();

                    // =========================
                    // Skapa betyg
                    // =========================
                    var newGrade = new CourseGrade
                    {
                        StudntId = student.StudentId,
                        CourseId = course.CourseId,
                        TeacherId = teacher.PersonalId,
                        Grade = gradeValue,
                        SetDate = DateOnly.FromDateTime(DateTime.Now)
                    };

                    context.CourseGrades.Add(newGrade);

                    
                    var more = AnsiConsole.Confirm("Vill du sätta fler betyg?");
                    if (!more)
                        break;
                }

                context.SaveChanges();
                transaction.Commit();

                AnsiConsole.MarkupLine("[green]Alla betyg sparades korrekt![/]");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AnsiConsole.MarkupLine("[red]Ett fel uppstod. Inga betyg sparades.[/]");
                AnsiConsole.MarkupLine($"[grey]{ex.Message}[/]");
            }

            Console.ReadKey();
        }











    }
}