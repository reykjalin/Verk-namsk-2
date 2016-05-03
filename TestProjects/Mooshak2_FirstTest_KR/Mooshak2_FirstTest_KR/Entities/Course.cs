using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Entities
{
    /// <summary>
    /// Course objects contain information on some course.
    /// Each course has an ID, a title and description. Each course will also have some assignments
    /// referenced to from the Assignment object (no information on assignments in the Course object
    /// itself.
    /// </summary>
    public class Course
    {
        [Key]
        public uint courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}