using NotaSatisSistemi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NotaSatisSistemi.Controllers
{
    public class KrediController : Controller
    {
        bool durum = false;
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {
                if (this.ControllerContext.RouteData.Values["id"].ToString() == "201")
                    durum = true;
            }
            catch (Exception)
            {

            }

        }
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
        Db_NotaSatisEntities db = new Db_NotaSatisEntities();
        // GET: Kredi
        [Authorize]
        public ActionResult Index()
        {
            if (durum)
                ViewBag.durum = "İşlem Başarılı. Hesabınıza kredi yüklenmiştir.";
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            int uID = 0;
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            Tbl_Kullanicilar user = db.Tbl_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == str);            
            uID = user.KullaniciID;            
            Tbl_Kullanicilar kullanici = db.Tbl_Kullanicilar.Find(uID);
            
            return View(kullanici);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCredit(string returnUrl, decimal Kredi)
        {
            string stx = HttpContext.Request.Url.Query;
            string str = System.Web.HttpContext.Current.User.Identity.Name;
            string _a = HttpContext.Request.Url.PathAndQuery;
            string sorgu = "update Tbl_Kullanicilar Set Kredi=(Tbl_Kullanicilar.Kredi +" + Kredi + ") where KullaniciAdi='" + str + "'";
            string sorguDuzenle = sorgu.Substring(sorgu.IndexOf("Kredi=") + 6, sorgu.IndexOf(")") - (sorgu.IndexOf("Kredi=") + 6)).Replace(",", ".");
            sorgu = sorgu.Replace(sorgu.Substring(sorgu.IndexOf("Kredi=") + 6, sorgu.IndexOf(")") - (sorgu.IndexOf("Kredi=") + 6)), sorguDuzenle);
            using (var context = new Db_NotaSatisEntities())
            {
                context.Database.ExecuteSqlCommand(sorgu);
            }
            //ReturnUrl
            if (returnUrl.Contains("BuyProduct"))
            {
                returnUrl =  ("~/" + returnUrl.Substring(returnUrl.IndexOf("=") + 1)).Replace("%2F","/");
                return Redirect(returnUrl);
            }
            ViewBag.durum = "İşlem Başarılı. Hesabınıza " + Kredi + " yüklenmiştir.";
            return RedirectToAction("Index", new { id = "201" });
        }
    }
}