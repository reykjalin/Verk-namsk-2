namespace Mooshak2_FirstTest_KR.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssignmentPart")]
    public partial class AssignmentPart
    {
        public int id { get; set; }

        public int partNr { get; set; }

        [Required]
        public string description { get; set; }

        public int weight { get; set; }

        public int assignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
