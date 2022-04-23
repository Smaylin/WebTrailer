using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrailerWeb.Permisos;
namespace TrailerWeb.Controllers
{
    //[ValidarSesion] Esto es lo que hace que no se pueda registrar a la pagina si no se esta logueado 
    public class HomeController : Controller
    {
        private Db_MOVIEEntities db = new Db_MOVIEEntities();
        public ActionResult Index()
        {
            return View(db.dbTrailers.ToList());
        }

        public ActionResult Movie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dbTrailers dbTrailers = db.dbTrailers.Find(id);
            if (dbTrailers == null)
            {
                return HttpNotFound();
            }
            return View(dbTrailers);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Trailers()
        {
            return RedirectToAction("Agregar", "Home");
        }

        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }
        [ValidateInput(false)]
        //[AllowHtml]
        public ActionResult Contact(int? id)
        {
            ViewBag.Message = "Your contact page.";

            List <Models.Trailer> objTrailer = null;


            Db_MOVIEEntities Db = new Db_MOVIEEntities();
            //int IdTr = 3;

            var MdlTrailer = db.dbTrailers.Where(Tr => Tr.ID_Trailer == id).FirstOrDefault();

            return View(MdlTrailer);

            //return View(objTrailer);
        }

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

    }
}
