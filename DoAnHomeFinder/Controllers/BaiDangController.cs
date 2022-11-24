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
    public class BaiDangController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "bBu9OWLGLienv1y8gTsD4g5qZ4oYkiLY2ZEP8vSa",
            BasePath = "https://homefinder1-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        // GET: BaiDang
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
            catch(Exception)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            ViewBag.quyen = Session["Quyen"].ToString();
            //End check session

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("room");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<BaiDang>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<BaiDang>(((JProperty)item).Value.ToString()));
            }
            var listReturn = new List<BaiDang>();
            if(list.Count() > 0)
            {
                for(int i = list.Count() - 1; i >= 0; i--)
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
        public ActionResult Create(BaiDang baidang)
        {
            try
            {
                addBaiDangToFireBase(baidang);
                ModelState.AddModelError(string.Empty, "Thêm bài đăng thành công");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        private void addBaiDangToFireBase(BaiDang baidang)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = baidang;
            data.list_image = new List<string>();
            PushResponse response = client.Push("room/", data);
            data.id_bai_dang = response.Result.name;
            SetResponse setResponse = client.Set("room/" + data.id_bai_dang, data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("room/" + id);
            BaiDang data = JsonConvert.DeserializeObject<BaiDang>(response.Body);
            List<string> image = new List<string>();
            try
            {
                if (data.list_image == null)
                {
                    image.Add("Không có ảnh");
                }
                else
                {
                    image = data.list_image;
                }
            }
            catch (Exception)
            {
            }
            ViewBag.trangthaiduyet = data.trang_thai_duyet;
            ViewBag.image = image;
            return View(data);
        }

            [HttpGet]
            public ActionResult Edit(string id)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("room/" + id);
                BaiDang data = JsonConvert.DeserializeObject<BaiDang>(response.Body);
                return View(data);
            }

        [HttpPost]
        public ActionResult Edit(BaiDang baidang)
        {
            //Get list image
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("room/" + baidang.id_bai_dang);
            BaiDang dataImg = JsonConvert.DeserializeObject<BaiDang>(response.Body);
            var list_img = new List<string>();
            list_img = dataImg.list_image;
            //END Get list image

            var data = baidang;
            data.list_image = list_img;
            SetResponse setResponse = client.Set("room/" + data.id_bai_dang, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse setResponse = client.Delete("room/" + id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Duyet(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("room/" + id);
            BaiDang data = JsonConvert.DeserializeObject<BaiDang>(response.Body);
            data.trang_thai_duyet = true;
            SetResponse setResponse = client.Set("room/" + data.id_bai_dang, data);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult BoDuyet(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("room/" + id);
            BaiDang data = JsonConvert.DeserializeObject<BaiDang>(response.Body);
            data.trang_thai_duyet = false;
            SetResponse setResponse = client.Set("room/" + data.id_bai_dang, data);
            return RedirectToAction("Index");
        }
    }
}