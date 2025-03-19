using NotaSatisSistemi.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NotaSatisSistemi.Controllers
{
    public class NotaController : Controller
    {
        Db_NotaSatisEntities db = new Db_NotaSatisEntities();
        public string yetkiKontrol()
        {
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
        // GET: Nota
        public ActionResult Index()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            return View(db.Tbl_Notalar.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                string yetki = yetkiKontrol();
                if (yetki == "yetkili")
                {
                    ViewBag.yetki = true;
                }
                Tbl_Notalar nota = db.Tbl_Notalar.Find(id);
                return View(nota);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult BuyProduct(int id)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            Tbl_Notalar nota = db.Tbl_Notalar.Find(id);            
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            if (str != "")
            {
                Tbl_Kullanicilar kullanici = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
                ViewData["KullaniciKredi"] = kullanici.Kredi;
            }
            else
                RedirectToAction("Login", "Guvenlik");

            return View(nota);
        }
        [Authorize]
        [HttpPost]
        public ActionResult BuyProduct(int NotaID, string KullaniciAdi, decimal Kredi)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            Tbl_Notalar nota = db.Tbl_Notalar.Find(NotaID);
            if (nota.Kredi != Kredi)
            {
                ViewBag.Hata = "Ürün fiyatı değiştiği için işleminiz iptal edildi. Ürün fiyatını kontrol edip sipariş verebilirsiniz.";
                return View();
            }
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            if (str == "")
            {
                RedirectToAction("Login", "Guvenlik");
            }
            if (KullaniciAdi != str)
            {
                ViewBag.Hata = "Giriş yapan kullanıcı değiştiği için işleminiz iptal edildi. Hesabı kontrol edip sipariş verebilirsiniz.";
                return View();
            }
            Tbl_Satislar satis = new Tbl_Satislar();
            if (KullaniciAdi == str)
            {
                Tbl_Kullanicilar kullanici = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
                satis.KullaniciID = kullanici.KullaniciID;
                using (var context = new Db_NotaSatisEntities())
                {
                    satis.NotaID = NotaID;
                    satis.Ucret = (decimal)context.Tbl_Notalar.Where(x => x.NotaID == NotaID).FirstOrDefault().Kredi;
                    satis.SatisTarihi = DateTime.Now;
                    db.Tbl_Satislar.Add(satis);
                    db.SaveChanges();
                }
                using (var context = new Db_NotaSatisEntities())
                {
                    kullanici.Kredi = kullanici.Kredi - satis.Ucret;
                    db.SaveChanges();
                }

            }                        
            ViewBag.Satis = @"Satış başarılı bir şekilde tamamlanmıştır. Satın aldığınız notanıza ""Notalarım"" sayfasından erişebilirsiniz.";
            return View(nota);
        }
        [Authorize]
        public ActionResult Order()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            int kID = 0;
            Tbl_Kullanicilar kullanici = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
            kID = kullanici.KullaniciID;
            var orders = db.Tbl_Satislar.Where(x => x.KullaniciID == kID);
            return View(orders.ToList());
        }
    }
}