using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Spectre.Console;
using lapp3.Data;

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
    }
}





