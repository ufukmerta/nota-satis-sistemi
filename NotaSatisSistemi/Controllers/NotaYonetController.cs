using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotaSatisSistemi.Models;

namespace NotaSatisSistemi.Controllers
{
    public class NotaYonetController : Controller
    {
        private Db_NotaSatisEntities db = new Db_NotaSatisEntities();
        public string yetkiKontrol()
        {
            Db_NotaSatisEntities db = new Db_NotaSatisEntities();
            bool yetkili = false;
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            if (str != "")
            {
                Tbl_Kullanicilar data = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
                yetkili = data.Yetkili;
                if (yetkili == true)
                {
                    return "yetkili";
                }
                else
                {
                    return "yetkisiz";
                }
            }
            else
            {
                return "girisyapilmamis";
            }
        }
        // GET: NotaYonet
        [Authorize]
        public ActionResult Index()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                return View(db.Tbl_Notalar.ToList());
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }

        // GET: NotaYonet/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tbl_Notalar notalar = db.Tbl_Notalar.Find(id);
                if (notalar == null)
                {
                    return HttpNotFound();
                }
                return View(notalar);
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }

        // GET: NotaYonet/Create
        [Authorize]
        public ActionResult Create()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                return View();
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NotaID,NotaAd,EserSahibi,Notalayan,NotaDetay,Enstruman,Kredi")] Tbl_Notalar tbl_Notalar)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                if (ModelState.IsValid)
                {
                    db.Tbl_Notalar.Add(tbl_Notalar);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tbl_Notalar);
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }

        // GET: NotaYonet/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tbl_Notalar notalar = db.Tbl_Notalar.Find(id);
                if (notalar == null)
                {
                    return HttpNotFound();
                }
                return View(notalar);
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NotaID,NotaAd,EserSahibi,Notalayan,NotaDetay,Enstruman,Kredi")] Tbl_Notalar tbl_Notalar)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                if (ModelState.IsValid)
                {
                    db.Entry(tbl_Notalar).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tbl_Notalar);
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }

        // GET: NotaYonet/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tbl_Notalar notalar = db.Tbl_Notalar.Find(id);
                if (notalar == null)
                {
                    return HttpNotFound();
                }
                return View(notalar);
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }

        // POST: NotaYonet/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                Tbl_Notalar notalar = db.Tbl_Notalar.Find(id);
                db.Tbl_Notalar.Remove(notalar);
                db.SaveChanges();
                //resim silme
                string ImageFileName = id + ".jpg";
                string FolderPath = System.IO.Path.Combine(Server.MapPath("~/NotaImages/"), ImageFileName);
                if (System.IO.File.Exists(FolderPath))
                {
                    System.IO.File.Delete(FolderPath);
                }
                //resim silindi
                return RedirectToAction("Index");
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult SaveImages()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                return View();
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SaveImages(string hiddenId, HttpPostedFileBase UploadedImages)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
                if (UploadedImages.ContentLength > 0)
                {
                    try
                    {
                        var ImageFileName = hiddenId + ".jpg";
                        var FolderPath = Path.Combine(Server.MapPath("~/NotaImages"), ImageFileName);
                        UploadedImages.SaveAs(FolderPath);
                        ViewBag.Durum = ImageFileName + " adlı fotoğraf yüklemesi başarılı.";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Durum = ex.Message;
                    }
                }
                return View();
            }
            else if (yetki == "yetkisiz")
            {
                return RedirectToAction("Login", "Guvenlik", new { id = "401" });
            }
            else
            {
                return RedirectToAction("Login", "Guvenlik");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
