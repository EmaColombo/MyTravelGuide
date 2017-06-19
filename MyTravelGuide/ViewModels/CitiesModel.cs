using RepositorioClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CitiesModel
    {
        public Cities City { get; set; }
        public List<ImagesCities> Images { get; set; }
    }

    public class ImagesCitiesModel
    {
        public Cities City { get; set; }
        public List<ImagesCities> Images { get; set; }
    }
}
