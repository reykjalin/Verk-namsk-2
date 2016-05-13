using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakV2.ViewModels
{
    public class HistoryViewModel
    {

        public int id { get; set; }

        public string userName { get; set; }

        public string course { get; set; }

        public string assignment { get; set; }

        public string assignmentPart { get; set; }

        public int success { get; set; }

        public int count { get; set; }

        public string filename { get; set; }

        public DateTime date { get; set; }

    }
}