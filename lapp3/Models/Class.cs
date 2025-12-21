using System;
using System.Collections.Generic;

namespace lapp3.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string? ClassName { get; set; }

    public int? ResponsibleTeacherId { get; set; }

    public virtual Personal? ResponsibleTeacher { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
