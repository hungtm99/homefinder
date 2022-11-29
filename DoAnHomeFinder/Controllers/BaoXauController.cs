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
    public class BaoXauController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "bBu9OWLGLienv1y8gTsD4g5qZ4oYkiLY2ZEP8vSa",
            BasePath = "https://homefinder1-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        // GET: BaoXau
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

            ViewBag.quyen = Session["Quyen"].ToString();
            //End check session

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("report");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<BaoXau>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<BaoXau>(((JProperty)item).Value.ToString()));
            }
            var listReturn = new List<BaoXau>();
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

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("report/" + id);
            BaoXau data = JsonConvert.DeserializeObject<BaoXau>(response.Body);
            return View(data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("report/" + id);
            BaoXau data = JsonConvert.DeserializeObject<BaoXau>(response.Body);
            return View(data);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse setResponse = client.Delete("report/" + id);
            return RedirectToAction("Index");
        }


    }
}