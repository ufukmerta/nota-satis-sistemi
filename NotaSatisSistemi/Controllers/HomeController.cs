using NotaSatisSistemi.Models;
using System.Linq;
using System.Web.Mvc;

namespace NotaSatisSistemi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
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
        public ActionResult Index()
        {
            string yetki = yetkiKontrol();
            if (yetki == "yetkili")
            {
                ViewBag.yetki = true;
            }
            return View();
        }
    }
}