namespace RepositorioClases
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Modelo : DbContext
    {
        public Modelo()
            : base("name=Modelo")
        {
        }

       
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UsersInRole> UsersInRoles { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder builder)
        {
        }
    }
}
