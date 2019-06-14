namespace WebBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
