namespace WebBook.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserDetailsModel: DbContext
    {
        public UserDetailsModel()
            : base("name=UserDetails")
        {
        }

        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<Post> Post { get; set; }

     

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
               .Property(e => e.Id)
               .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Content)
                .IsUnicode(false);
            modelBuilder.Entity<UserDetails>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.City)
                .IsUnicode(false);
        }



    }

    public class FriendsView
    {
        public UserDetails friend { get; set; }
        public Boolean friends { get; set; }
    }
}
