using NotaSatisSistemi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace NotaSatisSistemi.Controllers
{
    public class GuvenlikController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {
                if (this.ControllerContext.RouteData.Values["id"].ToString() == "401")
                {
                    ViewBag.Hata = "Yetkiniz olmayan yere giriş yaptınız.";
                }
                if (this.ControllerContext.RouteData.Values["id"].ToString() == "100")
                {
                    ViewBag.Hata = "Kullanıcı adı veya eposta zaten kayıtlı";
                }
            }
            catch (Exception)
            {
            }
        }
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
        // GET: Guvenlik
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string returnUrl, Tbl_Kullanicilar _Kullanici)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            var bilgiler = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == _Kullanici.KullaniciAdi && x.Sifre == _Kullanici.Sifre);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.KullaniciAdi, false);
                if (returnUrl != "/Guvenlik/Login")
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre yanlış!";
                return View();
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Tbl_Kullanicilar _Kullanici)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            var bilgiler = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == _Kullanici.KullaniciAdi || x.Eposta == _Kullanici.Eposta);
            if (bilgiler != null)
            {
                return RedirectToAction("Login", "Guvenlik", new { id = 100 });
            }
            else
            {
                db.Tbl_Kullanicilar.Add(_Kullanici);
                db.SaveChanges();
                return RedirectToAction("Login", "Guvenlik");
            }

        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string Kullaniciadi, string Telefon)
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            var bilgiler = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == Kullaniciadi && x.Telefon == Telefon);
            if (bilgiler != null)
            {
                ViewBag.Sifre = bilgiler.Sifre;
                return View();
            }
            else
            {
                ViewBag.Hata = "Kullanıcı adı veya telefon yanlış!";
                return View();
            }

        }
    }
}