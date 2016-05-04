namespace Mooshak2_FirstTest_KR.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int courseId { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string description { get; set; }
    }
}
