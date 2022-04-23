using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrailerWeb.Models;

namespace TrailerWeb.Controllers
{
    public class TrailerController : Controller
    {
        static string cadena = @"Data Source = BRYAN\SQLEXPRESS; Initial Catalog = Db_MOVIE;
        User= sa; Password=123456";

        public ActionResult Trailer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Agregar(Trailer oTrailer)
        {
            //Metodo para agregar
            try
            {
                string sp = "SpTrailersInsertar";
                SqlConnection cnn = new SqlConnection(cadena);
                SqlCommand cmd = new SqlCommand(sp, cnn);
                cnn.Open();
                cmd.Parameters.AddWithValue("TITULO",oTrailer.Titulo);
                cmd.Parameters.AddWithValue("DIRECTOR", oTrailer.Director);
                cmd.Parameters.AddWithValue("SIPNOSIS", oTrailer.Sipnosis);
                cmd.Parameters.AddWithValue("REPARTO", oTrailer.Reparto);
                cmd.Parameters.AddWithValue("DURACION", oTrailer.duracion);
                cmd.Parameters.AddWithValue("FECHA", oTrailer.Fecha);
                cmd.Parameters.AddWithValue("TRAILER", oTrailer._Trailer);
                cmd.Parameters.AddWithValue("IMAGEN_LOGO", oTrailer.imagen_logo);
                cmd.ExecuteNonQuery();
                /*cmd.Parameters.Add(new SqlParameter("@TITULO",oTrailer.Titulo));
                cmd.Parameters.Add(new SqlParameter("@DIRECTOR", oTrailer.Director));
                cmd.Parameters.Add(new SqlParameter("@SIPNOSIS", oTrailer.Sipnosis));
                cmd.Parameters.Add(new SqlParameter("@REPARTO", oTrailer.Reparto));
                cmd.Parameters.Add(new SqlParameter("@DURACION", oTrailer.duracion));
                cmd.Parameters.Add(new SqlParameter("@FECHA", oTrailer.Fecha));
                cmd.Parameters.Add(new SqlParameter("@TRAILER", oTrailer._Trailer));
                cmd.Parameters.Add(new SqlParameter("@IMAGEN_LOGO", oTrailer.imagen_logo));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;*/
                cnn.Close();
                return RedirectToAction("Index", "Home");

            }catch(Exception ex)
            {
                return View();
                throw ex;
            }
        }

        public ActionResult Eliminar(Trailer oTrailer)
        {
            //Metodo para eliminar
            try
            {
                string sp = "SpTrailersEliminar";
                SqlConnection cnn = new SqlConnection(cadena);
                SqlCommand cmd = new SqlCommand(sp, cnn);

                cnn.Open();
                cmd.Parameters.Add(new SqlParameter("@ID_TRAILER", oTrailer.Id_trailer));
                cmd.ExecuteNonQuery();
                cnn.Close();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View();
                throw ex;
            }
        }
    }
}
