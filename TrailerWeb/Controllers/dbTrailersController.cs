using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TrailerWeb.Permisos;
namespace TrailerWeb.Controllers
{
    //[ValidarSesion]

    public class dbTrailersController : Controller
    {
        private Db_MOVIEEntities db = new Db_MOVIEEntities();

        // GET: dbTrailers
        public ActionResult Index()
        {
            return View(db.dbTrailers.ToList());
        }


        // GET: dbTrailers/Details/5
        public ActionResult Details(int? id)
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

        // GET: dbTrailers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: dbTrailers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Trailer,Titulo,Director,Sipnosis,Reparto,Duracion,Fecha,Trailer,img_logo")] dbTrailers dbTrailers)
        {
            HttpPostedFileBase FileBase = Request.Files[0]; 

            if(FileBase.ContentLength == 0)
            {
                ModelState.AddModelError("img_logo", "Es necesario seleccionar una imagen");
            }
            else
            {
                if (FileBase.FileName.EndsWith(".jpg"))
                {
                    WebImage image = new WebImage(FileBase.InputStream);

                    dbTrailers.img_logo = image.GetBytes();
                }
                else
                {
                    ModelState.AddModelError("img_logo", "El sistema solo acepta imagenes .JPG");
                   
                }

               
            }


            //HttpFileCollectionBase collectionBase = Request.Files;

            

            if (ModelState.IsValid)
            {
                db.dbTrailers.Add(dbTrailers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dbTrailers);
        }

        // GET: dbTrailers/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: dbTrailers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Trailer,Titulo,Director,Sipnosis,Reparto,Duracion,Fecha,Trailer,img_logo")] dbTrailers dbTrailers)
        {
            //byte[] imagenActual = null;

            dbTrailers   _Trailers = new dbTrailers (); 

            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase.ContentLength == 0)
            {
                _Trailers = db.dbTrailers.Find(dbTrailers.ID_Trailer);
                dbTrailers.img_logo = _Trailers.img_logo;
            }
            else
            {

                if (FileBase.FileName.EndsWith(".jpg"))
                {
                    WebImage image = new WebImage(FileBase.InputStream);

                    dbTrailers.img_logo = image.GetBytes();
                }
                else
                {
                    ModelState.AddModelError("img_logo", "El sistema solo acepta imagenes .JPG");
                }

            }

            if (ModelState.IsValid)
            {
                db.Entry(_Trailers).State = EntityState.Detached;
                db.Entry(dbTrailers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dbTrailers);
        }

        // GET: dbTrailers/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: dbTrailers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dbTrailers dbTrailers = db.dbTrailers.Find(id);
            db.dbTrailers.Remove(dbTrailers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

            return File(memoryStream,"image/jpg");
        }

    }
}
