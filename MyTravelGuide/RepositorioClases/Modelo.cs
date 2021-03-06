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

        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UsersInRole> UsersInRoles { get; set; }
        public virtual DbSet<VotosUsersEvents> VotosUserEvents { get; set; }
        public virtual DbSet<EventsReportes> EventsReportes { get; set; }
        public virtual DbSet<PuntuacionesEventos> PuntuacionesEventos { get; set; }
        public virtual DbSet<InteresesEventos> InteresesEventos { get; set; }
        public virtual DbSet<UsersReportes> UsersReportes { get; set; }
        public virtual DbSet<TravelGuides> TravelGuides { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<ImagesCities> ImagesCities { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {

            builder.Entity<Events>().HasKey(q => q.Id);
            builder.Entity<Users>().HasKey(q => q.Id);
            builder.Entity<VotosUsersEvents>().HasKey(q => q.Id);

            // Relationships
            builder.Entity<VotosUsersEvents>()
                .HasRequired(t => t.Eventos)
                .WithMany(t => t.Votos)
                .HasForeignKey(t => t.IdEvent);

            builder.Entity<VotosUsersEvents>()
                .HasRequired(t => t.Usuarios)
                .WithMany(t => t.Votos)
                .HasForeignKey(t => t.IdUser);

        }
    }
}
