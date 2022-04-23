using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TrailerWeb.Controllers;
using TrailerWeb.Models;

namespace TrailerWeb.Controllers
{
    
    public class AccesoController : Controller
    {
        private Db_MOVIEEntities db = new Db_MOVIEEntities();

        static string cadena = "Data Source=LAPTOP-TQK3E9QQ;Initial Catalog=Db_MOVIE;Integrated Security=true";

        //conexion Smaylin
        //"Data Source=LAPTOP-TQK3E9QQ;Initial Catalog=Db_MOVIE;Integrated Security=true";

        //conexion hector
        //     @"Data Source = BRYAN\SQLEXPRESS; Initial Catalog = Db_MOVIE;
        //User= sa; Password=123456";
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View(db.dbTrailers.ToList());
        }

        #region metodos de Hector
        public ActionResult Trailer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(Trailer oTrailer)
        {
            //Metodo para agregar
            try
            {
                string sp = "SpTrailersInsertar";
                SqlConnection cnn = new SqlConnection(cadena);
                SqlCommand cmd = new SqlCommand(sp, cnn);
                cnn.Open();
                cmd.Parameters.Add(new SqlParameter("@TITULO", oTrailer.Titulo));
                cmd.Parameters.Add(new SqlParameter("@DIRECTOR", oTrailer.Director));
                cmd.Parameters.Add(new SqlParameter("@SIPNOSIS", oTrailer.Sipnosis));
                cmd.Parameters.Add(new SqlParameter("@REPARTO", oTrailer.Reparto));
                cmd.Parameters.Add(new SqlParameter("@DURACION", oTrailer.duracion));
                cmd.Parameters.Add(new SqlParameter("@FECHA", oTrailer.Fecha));
                cmd.Parameters.Add(new SqlParameter("@TRAILER", oTrailer._Trailer));
                cmd.Parameters.Add(new SqlParameter("@IMAGEN_LOGO", oTrailer.imagen_logo));
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
        #endregion

        //#region metodos Smaylin
        //[HttpPost]
        //public ActionResult Registrar(Usuario oUsuario)
        //{
        //    bool registrado;
        //    string mensaje;

        //    if (oUsuario.clave == oUsuario.ConfirmarClave)
        //    {

        //        oUsuario.clave = ConvertirSha256(oUsuario.clave);
        //    }
        //    else
        //    {
        //        ViewData["Mensaje"] = "Las contraseñas no coinciden";
        //        return View();
        //    }

        //    using (SqlConnection cn = new SqlConnection(cadena))
        //    {

        //        SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
        //        cmd.Parameters.AddWithValue("Correo", oUsuario.correo);
        //        cmd.Parameters.AddWithValue("Clave", oUsuario.clave);
        //        cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cn.Open();

        //        cmd.ExecuteNonQuery();

        //        registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
        //        mensaje = cmd.Parameters["Mensaje"].Value.ToString();


        //    }

        //    ViewData["Mensaje"] = mensaje;

        //    if (registrado)
        //    {
        //        return RedirectToAction("Login", "Acceso"); 
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}
        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
           {  
            oUsuario.clave = ConvertirSha256(oUsuario.clave);

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", oUsuario.correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.clave);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                oUsuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (oUsuario.IdUsuario != 0)
            {

                Session["usuario"] = oUsuario;
                return RedirectToAction("Index", "dbTrailers");
            }
            else
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                return View();
            }


        }
        public static string ConvertirSha256(string texto)
        {
            //using System.Text;
            //USAR LA REFERENCIA DE "System.Security.Cryptography"

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        //#endregion

        public ActionResult getImage(int id)
        {
            dbTrailers trailer = db.dbTrailers.Find(id);
            byte[] byteImage = trailer.img_logo;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image img = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();

            img.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpg");
        }

        //public ActionResult Contact(int? id)
        //{
        //    ViewBag.Message = "Your contact page.";

        //    List<Models.Trailer> objTrailer = null;


        //    Db_MOVIEEntities Db = new Db_MOVIEEntities();
        //    //int IdTr = 3;

        //    var MdlTrailer = db.dbTrailers.Where(Tr => Tr.ID_Trailer == id).FirstOrDefault();

        //    return View(MdlTrailer);

        //    //return View(objTrailer);
        //}

    }
}