using NotaSatisSistemi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
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
            Tbl_Notalar notalar = db.Tbl_Notalar.Find(id);
            ViewData["Eser"] = notalar.NotaAd;
            ViewData["EserSahibi"] = notalar.EserSahibi;
            ViewData["Notalayan"] = notalar.Notalayan;
            ViewData["Enstruman"] = notalar.Enstruman;
            ViewData["Kredi"] = notalar.Kredi;
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            if (str != "")
            {
                Tbl_Kullanicilar data = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
                ViewData["KullaniciKredi"] = data.Kredi;
            }
            else
                RedirectToAction("Login", "Guvenlik");

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult BuyProduct(Tbl_Satislar satis, string KullaniciAdi, decimal Kredi)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            Tbl_Notalar notalar = db.Tbl_Notalar.Find(satis.NotaID);
            if (notalar.Kredi != Kredi)
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
            if (KullaniciAdi == str)
            {
                Tbl_Kullanicilar data = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
                satis.KullaniciID = data.KullaniciID;

            }
            satis.Ucret = Kredi;
            satis.SatisTarihi = DateTime.Now;
            db.Tbl_Satislar.Add(satis);
            db.SaveChanges();
            string sorgu = "update Tbl_Kullanicilar Set Kredi=(Tbl_Kullanicilar.Kredi - " + Kredi + ") where KullaniciAdi='" + str + "'";
            string sorguDuzenle = sorgu.Substring(sorgu.IndexOf("Kredi=") + 6, sorgu.IndexOf(")") - (sorgu.IndexOf("Kredi=") + 6)).Replace(",", ".");
            sorgu = sorgu.Replace(sorgu.Substring(sorgu.IndexOf("Kredi=") + 6, sorgu.IndexOf(")") - (sorgu.IndexOf("Kredi=") + 6)), sorguDuzenle);
            //sorgu = sorgu.Replace(",", "."); basit yapılı versiyonu
            using (var context = new Db_NotaSatisEntities())
            {
                context.Database.ExecuteSqlCommand(sorgu);
            }
            ViewBag.Satis = @"Satış başarılı bir şekilde tamamlanmıştır. Satın aldığınız notanıza ""Notalarım"" sayfasından erişebilirsiniz.";
            ViewData["Eser"] = notalar.NotaAd;
            ViewData["EserSahibi"] = notalar.EserSahibi;
            ViewData["Notalayan"] = notalar.Notalayan;
            ViewData["Enstruman"] = notalar.Enstruman;
            ViewData["Kredi"] = notalar.Kredi;
            return View();
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
            Tbl_Kullanicilar user = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);
            kID = user.KullaniciID;
            DbSqlQuery<Tbl_Satislar> orders = db.Tbl_Satislar.SqlQuery("select * from Tbl_Satislar where KullaniciID= @p0", kID);
            return View(orders.ToList());
        }
    }
}