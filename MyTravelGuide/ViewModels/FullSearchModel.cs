using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class FullSearchModel
    {

        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public long Id { get; set; }
        public string Categoria { get; set; }
        public string NombreEvento { get; set; }
        public string Usuario { get; set; }
        public int IdUser { get; set; }
        public int? IdCategoria { get; set; }
        public string EncontradoEn { get; set; }
    }

    public class FullModel
    {
        public List<FullSearchModel> Lista { get; set; }

        public string searchString { get; set; }
    }
}
