using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Entities
{
    /// <summary>
    /// Each Assignment object should have a list of AssignmentPart objects
    /// that describe each part of the assignment
    /// </summary>
    public class AssignmentPart
    {
        public uint assignmentId { get; set; }
        public uint courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public uint weightInAssignment { get; set; }
    }
}