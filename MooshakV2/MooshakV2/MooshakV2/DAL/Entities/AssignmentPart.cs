namespace MooshakV2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssignmentPart")]
    public partial class AssignmentPart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssignmentPart()
        {
            Submissions = new HashSet<Submission>();
        }

        public int id { get; set; }

        public int partNr { get; set; }

        [Required]
        public string description { get; set; }

        public int weight { get; set; }

        public int assignmentId { get; set; }

        [Required]
        public string title { get; set; }

        public virtual Assignment Assignment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
