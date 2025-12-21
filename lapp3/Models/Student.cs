using System;
using System.Collections.Generic;

namespace lapp3.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? FristName { get; set; }

    public string? LastName { get; set; }

    public string? SocialSecurityNumber { get; set; }

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<CourseGrade> CourseGrades { get; set; } = new List<CourseGrade>();
}
