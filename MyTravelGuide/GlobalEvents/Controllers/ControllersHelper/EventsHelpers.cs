using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalEvents.Helpers
{
    public static class EventsHelpers
    {
        public static string getIcon(this HtmlHelper html, string Categoria)
        {
            switch (Categoria)
            {
                case "Musica":
                    return "fa fa-music fa-fw";
                    break;
                case "Fiestas":
                    return "fa fa-beer fa-fw";
                    break;
                case "Artes":
                    return "fa fa-picture-o fa-fw";
                    break;
                case "Gastronomia":
                    return "fa fa-cutlery fa-fw";
                    break;
                case "Clases":
                    return "fa fa-graduation-cap fa-fw";
                    break;
                case "Deportes":
                    return "fa fa-futbol-o fa-fw";
                    break;
                case "Otros":
                    return "fa fa-folder-o fa-fw";
                    break;
                default:
                    return "fa fa-exclamation fa-fw";
                    break;
            }
        }

        public static string getEvent(this HtmlHelper html, string id)
        {
            List<RepositorioClases.Events> Evento = Servicios.EventsService.ObtenerEventos(long.Parse(id));
            String Name = "";
            if (Evento.Count > 0)
                Name = Evento.First().NombreEvento;
            return Name == "" ? "No existe" : Name;
        }
    }
}