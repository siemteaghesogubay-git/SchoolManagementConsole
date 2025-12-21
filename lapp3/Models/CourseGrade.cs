using System;
using System.Collections.Generic;

namespace lapp3.Models;

public partial class CourseGrade
{
    public int GradeId { get; set; }

    public string? Grade { get; set; }

    public int? CourseId { get; set; }

    public int? StudntId { get; set; }

    public int? TeacherId { get; set; }

    public DateOnly? SetDate { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Studnt { get; set; }

    public virtual Personal? Teacher { get; set; }
}
