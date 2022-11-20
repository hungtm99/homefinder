using DoAnHomeFinder.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnHomeFinder.Controllers
{
    public class LoginController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "bBu9OWLGLienv1y8gTsD4g5qZ4oYkiLY2ZEP8vSa",
            BasePath = "https://homefinder1-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        // GET: Login
        public ActionResult Index()
        {
            Session["Email"] = "";
            Session["Quyen"] = "";
            ViewBag.quyen = "";
            return View();
        }

        public ActionResult Logout()
        {
            Session["Email"] = "";
            Session["Quyen"] = "";
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        [HttpPost]
        public ActionResult Index(NguoiDung NguoiDung)
        {
            try
            {
                var Email = NguoiDung.email;
                var MatKhau = NguoiDung.mat_khau;

                if (checkLogin(Email, MatKhau))
                {
                    return RedirectToAction("Index", "BaiDang", new { area = "" });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        private bool checkLogin(string Email, string MatKhau)
        {

            //Get all user
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("user");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var lstUser = new List<NguoiDung>();
            foreach (var item in data)
            {
                lstUser.Add(JsonConvert.DeserializeObject<NguoiDung>(((JProperty)item).Value.ToString()));
            }
            //End get all user

            bool check = false;

            if(lstUser.Count() > 0)
            {
                for (int i = 0; i < lstUser.Count(); i++)
                {
                    if (lstUser[i].email == Email && lstUser[i].mat_khau == MatKhau && lstUser[i].quyen != 0)
                    {
                        Session["Email"] = Email;
                        Session["Quyen"] = lstUser[i].quyen;
                        return true;
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác, vui lòng thử lại !!! ");
            return check;
        }
    }
}