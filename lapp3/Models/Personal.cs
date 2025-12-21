using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;

namespace lapp3.Models;

public partial class Personal
{
    public int PersonalId { get; set; }

    public string? FristName { get; set; }

    public string? LastName { get; set; }

    public string? SocialSecurityNumber { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<CourseGrade> CourseGrades { get; set; } = new List<CourseGrade>();
    
}
