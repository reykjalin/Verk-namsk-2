using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Entities
{
    public class Assignment
    {
        public uint assignmentId { get; set; }
        public uint courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public uint weight { get; set; }
    }
}