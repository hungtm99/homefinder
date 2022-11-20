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
    public class NguoiDungController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "bBu9OWLGLienv1y8gTsD4g5qZ4oYkiLY2ZEP8vSa",
            BasePath = "https://homefinder1-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        // GET: NguoiDung
        public ActionResult Index()
        {
            //Check session
            try
            {
                var checkSS = "";
                checkSS = Session["Email"] == null ? "" : "1";
                var session = string.IsNullOrEmpty(checkSS) ? "" : Session["Email"].ToString();
                if (string.IsNullOrEmpty(session))
                {
                    return RedirectToAction("Index", "Login", new { area = "" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            ViewBag.quyen = Session["Quyen"].ToString();

            //End check session

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("user");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<NguoiDung>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<NguoiDung>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NguoiDung NguoiDung)
        {
            try
            {
                addNguoiDungToFireBase(NguoiDung);
                ModelState.AddModelError(string.Empty, "Thêm người dùng thành công");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        private void addNguoiDungToFireBase(NguoiDung NguoiDung)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = NguoiDung;
            PushResponse response = client.Push("user/", data);
            data.id_nguoi_dung = response.Result.name;
            SetResponse setResponse = client.Set("user/" + data.id_nguoi_dung, data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("user/" + id);
            NguoiDung data = JsonConvert.DeserializeObject<NguoiDung>(response.Body);
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("user/" + id);
            NguoiDung data = JsonConvert.DeserializeObject<NguoiDung>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(NguoiDung NguoiDung)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = NguoiDung;
            SetResponse setResponse = client.Set("user/" + data.id_nguoi_dung, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if(Session["Quyen"].ToString() != "2")
            {
                ModelState.AddModelError(string.Empty, "Bạn không có quyền thực hiện hành động này");
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse setResponse = client.Delete("user/" + id);
            }
            return RedirectToAction("Index");
        }
    }
}