using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositorioClases
{
    public class States
    {
        public enum EventState
        {
            Habilitado = 1,
            Bloqueado = 2,
            Reportado = 3,
            Eliminado = 4,
            Pendiente_De_Aprobacion = 5
        }

        public enum UserState
        {
            Activo = 1,
            Bloqueado = 2,
            Reportado = 3,
            Eliminado = 4,
            Inactivo = 5
        }

        public enum TravelGuideState
        {
            Activo = 1,
            Eliminado = 2,
            Realizado = 3
        }

        public enum CityType
        {
            Origin = 1,
            Intermediate = 2,
            Destiny = 3,
            Evaluate = 4
        }
    }
}
