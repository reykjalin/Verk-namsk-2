namespace MooshakV2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Assignment")]
    public partial class Assignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assignment()
        {
            AssignmentParts = new HashSet<AssignmentPart>();
            Submissions = new HashSet<Submission>();
        }

        public int id { get; set; }

        public int courseId { get; set; }

        [Required]
        public string title { get; set; }

        public string description { get; set; }

        public int weight { get; set; }

        public DateTime? handInDate { get; set; }

        public string input { get; set; }

        public string output { get; set; }

        public virtual Course Course { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignmentPart> AssignmentParts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
