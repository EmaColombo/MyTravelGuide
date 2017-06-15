﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioClases
{
    public class Cities
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "City")]
        public string CityName { get; set; }

        [Required]
        [StringLength(50)]
        public string lat { get; set; }

        [Required]
        [StringLength(50)]
        public string lng { get; set; }


        [Required]
        [Display(Name = "User")]
        public int IdUser { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public long TravelGuideId { get; set; }

        [ForeignKey("TravelGuideId")]
        public virtual TravelGuides TravelGuide { get; set; }

        public States.CityType CityType { get; set; }

        public string PlaceId { get; set; }
    }
}
