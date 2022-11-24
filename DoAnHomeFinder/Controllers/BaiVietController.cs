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
    public class BaiVietController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "bBu9OWLGLienv1y8gTsD4g5qZ4oYkiLY2ZEP8vSa",
            BasePath = "https://homefinder1-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;
        // GET: BaiViet

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
            FirebaseResponse response = client.Get("post");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<BaiViet>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<BaiViet>(((JProperty)item).Value.ToString()));
            }
            var listReturn = new List<BaiViet>();
            if (list.Count() > 0)
            {
                for (int i = list.Count() - 1; i >= 0; i--)
                {
                    listReturn.Add(list[i]);
                }
            }
            return View(listReturn);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(BaiViet baiviet)
        {
            try
            {
                addBaiDangToFireBase(baiviet);
                ModelState.AddModelError(string.Empty, "Thêm bài viết thành công");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        private void addBaiDangToFireBase(BaiViet baiviet)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = baiviet;
            PushResponse response = client.Push("post/", data);
            data.id_bai_viet = response.Result.name;
            SetResponse setResponse = client.Set("post/" + data.id_bai_viet, data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("post/" + id);
            BaiViet data = JsonConvert.DeserializeObject<BaiViet>(response.Body);
            try
            {
                if (data.hinh_anh == null)
                {
                    ViewBag.image = " Không có ảnh";
                }
                else
                {
                    ViewBag.image = data.hinh_anh;
                }
            }
            catch (Exception)
            {
            }
            
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("post/" + id);
            BaiViet data = JsonConvert.DeserializeObject<BaiViet>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(BaiViet baiviet)
        {
            //Get list image
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("post/" + baiviet.id_bai_viet);
            BaiViet dataImg = JsonConvert.DeserializeObject<BaiViet>(response.Body);

            var data = baiviet;
            data.hinh_anh = dataImg.hinh_anh;
            SetResponse setResponse = client.Set("post/" + data.id_bai_viet, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse setResponse = client.Delete("post/" + id);
            return RedirectToAction("Index");
        }

    }
}