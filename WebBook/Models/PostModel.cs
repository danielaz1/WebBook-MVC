namespace WebBook.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PostModel : DbContext
    {
        public PostModel()
            : base("name=Post")
        {
        }

        public virtual DbSet<Post> Post { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Content)
                .IsUnicode(false);
        }
    }
}
