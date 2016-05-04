using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak2_FirstTest_KR.Models.ViewModels
{
    public class AssignmentViewModel
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int weight { get; set; }
    }
}