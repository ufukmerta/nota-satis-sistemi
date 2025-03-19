using NotaSatisSistemi.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotaSatisSistemi.Controllers
{
    public class KrediController : Controller
    {        
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

            string str = System.Web.HttpContext.Current.User.Identity.Name;
            Tbl_Kullanicilar kullanici = db.Tbl_Kullanicilar.Where(u => u.KullaniciAdi.Equals(str)).FirstOrDefault();
            kullanici.Kredi += Kredi;
            db.SaveChanges();

            //ReturnUrl
            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = HttpUtility.UrlDecode(returnUrl);
                return Redirect(returnUrl.Substring(returnUrl.IndexOf("=")+1));
            }
            TempData["durum"] = "İşlem Başarılı. Hesabınıza " + Kredi + " kredi yüklenmiştir.";
            Response.StatusCode = 204;
            TempData["statusCode"] = "204";
            return RedirectToAction("Index");
        }
    }
}