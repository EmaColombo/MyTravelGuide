namespace RepositorioClases
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public enum UserState
    {
        Activo = 1,
        Bloqueado = 2,
        Reportado = 3,
        Eliminado = 4,
        Inactivo = 5
    }

    public class Users
    {
        public Int32 Id { get; set; }

        [StringLength(20)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo usuario es obligatorio.")]
        public string Usuario { get; set; }

        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo email es obligatorio")]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio")]
        public UserState Estado { get; set; }

        public bool? UserDestacado { get; set; }

        [ForeignKey("IdUser")]
        public virtual List<VotosUsersEvents> Votos { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual List<CommentsReportes> Reportes { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual List<EventsReportes> ReportesEv { get; set; }
    }

    [Table("webpages_Membership")]
    public class Membership
    {
        public Membership()
        {
            //Roles = new List<Role>();
            OAuthMemberships = new List<OAuthMembership>();
            UsersInRoles = new List<UsersInRole>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(128)]
        public string ConfirmationToken { get; set; }
        public bool? IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        [Required, StringLength(128)]
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        [Required, StringLength(128)]
        public string PasswordSalt { get; set; }
        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        //public ICollection<Role> Roles { get; set; }

        [ForeignKey("UserId")]
        public ICollection<OAuthMembership> OAuthMemberships { get; set; }

        [ForeignKey("UserId")]
        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

    [Table("webpages_OAuthMembership")]
    public class OAuthMembership
    {
        [Key, Column(Order = 0), StringLength(30)]
        public string Provider { get; set; }

        [Key, Column(Order = 1), StringLength(100)]
        public string ProviderUserId { get; set; }

        public int UserId { get; set; }

        [Column("UserId"), InverseProperty("OAuthMemberships")]
        public Membership User { get; set; }
    }

    [Table("webpages_UsersInRoles")]
    public class UsersInRole
    {
        [Key, Column(Order = 0)]
        public int RoleId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [Column("RoleId"), InverseProperty("UsersInRoles")]
        public Role Roles { get; set; }

        [Column("UserId"), InverseProperty("UsersInRoles")]
        public Membership Members { get; set; }
    }

    [Table("webpages_Roles")]
    public class Role
    {
        public Role()
        {
            UsersInRoles = new List<UsersInRole>();
        }

        [Key]
        public int RoleId { get; set; }
        [StringLength(256)]
        public string RoleName { get; set; }

        //public ICollection<Membership> Members { get; set; }

        [ForeignKey("RoleId")]
        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

    public partial class UsersReportes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReporteId { get; set; }

        public long UserId { get; set; }

        public int IdUsuario { get; set; }

        [Required]
        [StringLength(300)]
        public string Observacion { get; set; }

        public DateTime Fecha { get; set; }

        public bool Resuelto { get; set; }
    }
}
