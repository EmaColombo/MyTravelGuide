using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioClases
{
    public class Countries
    {
        [Key]
        public Int32 CountryId { get; set; }

        [StringLength(100)]
        public string CountryName { get; set; }

        [MaxLength(2)]
        public string alpha2 { get; set; }
        [MaxLength(3)]
        public string alpha3 { get; set; }

    }

}
