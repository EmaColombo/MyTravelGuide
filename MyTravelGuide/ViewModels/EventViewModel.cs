using RepositorioClases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ViewModels
{
    public class EventViewModel
    {
        public class EventVM
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
            [Display(Name = "Inicio")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime FechaInicio { get; set; }

            [Display(Name = "Fin")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime FechaFin { get; set; }

            [Required]
            public int IdUser { get; set; }

            [Required]
            [Range(0, 5, ErrorMessage = "Seleccione Categoría Correcta")]
            public Categorias IdCategoria { get; set; }

            [Required]
            [Display(Name = "Evento Destacado")]
            public bool Destacado { get; set; }

            [Required]
            [StringLength(200)]
            public string Direccion { get; set; }

            [Required]
            [StringLength(200)]
            public string RutaImagen { get; set; }

            public Nullable<TimeSpan> HoraInicio { get; set; }

            public Nullable<TimeSpan> HoraFin { get; set; }

            [Required]
            public EventState Estado { get; set; }

        }

        public partial class EventCreateModel
        {
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
            [Range(0, 7, ErrorMessage = "Seleccione una categoría correcta")]
            public Categorias IdCategoria { get; set; }

            [Required]
            [StringLength(200)]
            public string Direccion { get; set; }

            public string Pais { get; set; }
            public string Ciudad { get; set; }

            public String RutaImagen { get; set; }

            [Required]
            public Nullable<TimeSpan> HoraInicio { get; set; }
            [Required]
            public Nullable<TimeSpan> HoraFin { get; set; }
        }

        public partial class EventModel
        {
            public EventVM ViewModel { get; set; }
            public int Puntuacion { get; set; }
            public Decimal Promedio { get; set; }
        }

    }

    public class EventosModeracionModel
    {
        public int ReporteId { get; set; }
        public int IdUsuario { get; set; }
        public long EventId { get; set; }
        public string Evento { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public Estado Estado { get; set; }
    }

    public class InteresesEventosModel
    {
        public InteresesViewModel InteresUsuario { get; set; }
        public Int64 Asistencias { get; set; }
        public Int64 Interesados { get; set; }
        public DateTime FechaFin { get; set; }
        public Int64 IdEvento { get; set; }
    }

    public class InteresesViewModel
    {
        
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
}
