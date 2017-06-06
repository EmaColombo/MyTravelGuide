using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioClases
{
    public class TravelGuides
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TravelGuideId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Travel Guide Name")]
        public string TravelGuideName { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public int IdUser { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        public String Image { get; set; }

        [Required]
        public States.EventState EventState { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }
}
