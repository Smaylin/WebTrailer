using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrailerWeb.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string ConfirmarClave { get; set; }
    }
}