﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Entities
{
    /// <summary>
    /// An Assignment object contains information about some assignment
    /// made by a teacher for a specific course
    /// </summary>
    public class Assignment
    {
        public uint assignmentId { get; set; }
        public uint courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public uint weight { get; set; }
    }
}