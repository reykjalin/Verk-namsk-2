namespace MooshakV2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Submission")]
    public partial class Submission
    {
        public int Id { get; set; }

        public DateTime date { get; set; }

        public int assignmentId { get; set; }

        public int partId { get; set; }

        [Required]
        [StringLength(128)]
        public string userId { get; set; }

        [Required]
        public string filename { get; set; }

        [Required]
        public string mime { get; set; }

        public int success { get; set; }

        public int count { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Assignment Assignment { get; set; }

        public virtual AssignmentPart AssignmentPart { get; set; }
    }
}
