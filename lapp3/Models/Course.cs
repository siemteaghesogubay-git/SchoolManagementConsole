using System;
using System.Collections.Generic;

namespace lapp3.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }
    public bool IsActive { get; set; }


    public virtual ICollection<CourseGrade> CourseGrades { get; set; } = new List<CourseGrade>();
}
