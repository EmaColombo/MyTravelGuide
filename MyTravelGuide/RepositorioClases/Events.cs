namespace RepositorioClases
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Events
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Evento")]
        public string NombreEvento { get; set; }

        [Required]
        [StringLength(50)]
        public string lat { get; set; }

        [Required]
        [StringLength(50)]
        public string lng { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha Finalización")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int IdUser { get; set; }

        [Required]
        [Range(0, 7, ErrorMessage = "Seleccione una categoría correcta")]
        public Categorias IdCategoria { get; set; }

        [Required]
        [Display(Name = "Evento Destacado")]
        public bool Destacado { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }


        //[Required]
        //[StringLength(200)]
        public String RutaImagen { get; set; }

        [ForeignKey("EventId")]
        public virtual List<Comments> Comments { get; set; }

        [Required]
        public EventState Estado { get; set; }

        public Nullable<TimeSpan> HoraInicio { get; set; }

        public Nullable<TimeSpan> HoraFin { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdEvent")]
        public virtual List<VotosUsersEvents> Votos { get; set; }

        [ForeignKey("EventId")]
        public virtual List<EventsReportes> Reportes { get; set; }
    }

    public partial class VotosUsersEvents
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int IdUser { get; set; }
        public long IdEvent { get; set; }
        public bool Voto { get; set; }

    
        public virtual Events Eventos { get; set; }

        
        public virtual Users Usuarios { get; set; }
    }

    public partial class EventsReportes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReporteId { get; set; }

        public long EventId { get; set; }

        public int IdUsuario { get; set; }

        [Required]
        [StringLength(300)]
        public string Observacion { get; set; }

        public DateTime Fecha { get; set; }

        public bool? Resuelto { get; set; }

        public virtual Events Events { get; set; }
        public virtual Users User { get; set; }
    }

    public partial class PuntuacionesEventos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPuntuacion { get; set; }
        [Required]
        public long EventId { get; set; }
        [Required]
        public int IdUsuario { get; set;}
        [Required]
        public int Puntuacion { get; set; }
    }

    public partial class InteresesEventos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdInteres { get; set; }
        [Required]
        public long EventId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public Intereses Tipo { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public bool Anulado { get; set; }
        public DateTime? FechaAnulacion { get; set; }
    }

    public enum EventState
    {
        Habilitado = 1,
        Bloqueado = 2,
        Reportado = 3,
        Eliminado = 4,
        Pendiente_De_Aprobacion = 5
    }

    public enum Categorias
    {
        Musica = 1,
        Fiestas = 2,
        Artes = 3,
        Gastronomia = 4,
        Clases = 5,
        Deportes = 6,
        Otros = 7
    }

    public enum Intereses
    {
        Asistire = 1,
        Me_Gusta = 2
    }

}
