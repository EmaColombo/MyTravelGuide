using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RepositorioClases;

namespace ViewModels
{
    public class CommentsModel
    {
        public List<Comments> CommentsList { get; set; }

        [Display(Name = "Comentario")]
        public string Comment { get; set; }

        public long IdEvento { get; set; }

        public long IdComment { get; set; }
    }

    public class Comments
    {
        [Display(Name = "ID")]
        public long CommentId { get; set; }

        [Required]
        [Display(Name = "Usuario creador")]
        public int iDUsuario { get; set; }

        public string Usuario { get; set; }

        [Required]
        [Display(Name = "Número evento")]
        public long EventId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public DateTime FechaUltimaActualizacion { get; set; }

        public long? ComentarioPadre { get; set; }

        [Required]
        [StringLength(3000, ErrorMessage = "No se permite que el comentario sea mayor a los 5000 carácteres")]
        public string Comentario { get; set; }

        public int Like { get; set; }

        public int UnLike { get; set; }

        public Estado Estado { get; set; }
    }

    public class ComentariosEventosModeracionModel
    {
        public long CommentId { get; set; }
        public int IdUsuario { get; set; }
        public long EventId { get; set; }
        public string Evento { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public Estado Estado { get; set; }
    }
}
