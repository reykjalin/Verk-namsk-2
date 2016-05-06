namespace MooshakV2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserDetail")]
    public partial class UserDetail
    {
        [Key]
        public string userId { get; set; }

        [Required]
        [StringLength(256)]
        public string userName { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string ssn { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
