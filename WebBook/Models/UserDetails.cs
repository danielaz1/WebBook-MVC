namespace WebBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDetails
    {
        [Key]
        public string Id { get; set; }

        [StringLength(128)]
        public string OwnerID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}
