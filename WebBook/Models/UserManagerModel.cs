namespace WebBook.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public partial class UserManagerModel : DbContext
    {
        public UserManagerModel()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

    }

    public class UserListViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Role")]
        public string Role { get; set; }
        public string Login { get; set; }

    }
    public class UserEditViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> roles { get; set; }
    }
}