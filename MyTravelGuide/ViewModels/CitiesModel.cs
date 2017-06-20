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
        public long TravelGuideId { get; set; }
    }

    public class ImagesCitiesModel
    {
        public Cities City { get; set; }
        public List<ImagesCities> Images { get; set; }
    }

    public class CitiesListModel
    {
        public List<Cities> Cities { get; set; }
        public long TravelGuideId { get; set; }
    }
}
