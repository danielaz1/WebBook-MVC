namespace WebBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Friends
    {
        [Key]
        [Column(Order = 0)]
        public string User1Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string User2Id { get; set; }
    }
}
