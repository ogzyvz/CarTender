using Infoline.WorkOfTimeManagement.BusinessAccess;
using Infoline.Web.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json;

using Infoline.Framework.Database;
using System.Net.Http;

namespace Infoline.WorkOfTimeManagement.WebProject.Areas.SYS.Controllers
{
    public class LanguageController : Controller
    {

        public static Dictionary<string, Dictionary<string, string>> Global = new Dictionary<string, Dictionary<string, string>>();

        public string cleanString(string str)
        {

            str = str.Trim().Replace(System.Environment.NewLine, " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");

            while (str.IndexOf("  ") > 0)
            {
                str = str.Replace("  ", " ");
            }

            return str;
        }

        [AllowEveryone]

        private void staticLoad()
        {

            var sourcedirectory = Server.MapPath("~/Language/Sources/");
            var recoverydirectory = Server.MapPath("~/Language/Recovery/");
            if (!System.IO.Directory.Exists(sourcedirectory))
            {
                System.IO.Directory.CreateDirectory(sourcedirectory);
            }
            if (!System.IO.Directory.Exists(recoverydirectory))
            {
                System.IO.Directory.CreateDirectory(recoverydirectory);
            }



            if (Global != null && Global.Count() == 0)
            {
                string text = System.IO.File.ReadAllText(Server.MapPath("/Language/Sources/global.json"));
                Global = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(text);
            }

        }

        public class Translation
        {
            public int code { get; set; }
            public string lang { get; set; }
            public List<string> text { get; set; }
        }
        private static readonly HttpClient client = new HttpClient();
        [AllowEveryone]
        public JsonResult LanguageLoad(string[] param)
        {
            try
            {
                string[] res1 = null;
                string[] res2 = null;

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["text"] = String.Join("<t>", param);
                    data["lang"] = "tr-en"; ;
                    data["format"] = "html";
                    data["key"] = "trnsl.1.1.20180706T133830Z.f183192ae0f59ab2.18a3c139e0ebb82ffb30c1cc5e18d535719cd522";
                    var response = wb.UploadValues("https://translate.yandex.net/api/v1.5/tr.json/translate", "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                    var rootObject = JsonConvert.DeserializeObject<Translation>(responseInString);

                    res1 = System.Text.RegularExpressions.Regex.Split(rootObject.text[0], "<t>");



                }
                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["text"] = String.Join("<t>", param);
                    data["lang"] = "tr-ru";
                    data["format"] = "html";
                    data["key"] = "trnsl.1.1.20180706T133830Z.f183192ae0f59ab2.18a3c139e0ebb82ffb30c1cc5e18d535719cd522";
                    var response = wb.UploadValues("https://translate.yandex.net/api/v1.5/tr.json/translate", "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                    var rootObject = JsonConvert.DeserializeObject<Translation>(responseInString);

                    res2 = System.Text.RegularExpressions.Regex.Split(rootObject.text[0], "<t>");


                }
                return Json(new ResultStatus { result = true, objects = new { TR_EN = res1, TR_RU = res2 } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResultStatus { result = false, objects = null, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [AllowEveryone]
        public ActionResult Index()
        {
            staticLoad();
            return View();
        }

        [AllowEveryone]
        public ContentResult GetPages()
        {

            var data = new LanguageSearch().GetAllPages()
                .Where(a => System.IO.File.Exists(Server.MapPath(String.Concat((string.IsNullOrEmpty(a.Area) ? "~" : "~/Areas/" + a.Area), "/Views/", a.Controller, "/", a.Action, ".cshtml"))))
                .Where(a => a.Attributes.IndexOf("HttpPost") == -1)
                .OrderBy(a => a.Area)
                .ToArray();

            var result = data.Select(a => new
            {
                Area = a.Area,
                Controller = a.Controller,
                Action = a.Action,
                Value = a.Area + "-" + a.Controller + "-" + a.Action + ".json",
                Text = "/" + (string.IsNullOrEmpty(a.Area) ? "" : a.Area + "/") + a.Controller + "/" + a.Action
            }).ToArray();
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }


        [AllowEveryone]
        [HttpPost]
        public Guid? GetParam(string Controller)
        {

            Guid? id = null;
            var db = new IntranetManagementDatabase();

            try
            {
                id = db.GetTableToId(Controller);
            }
            catch (Exception ex) { }

            return id;
        }




        [AllowEveryone]
        [HttpPost]
        public JsonResult Save(string json, string fileName)
        {

            try
            {

                var sourcedirectory = Server.MapPath("~/Language/Sources/");
                var recoverydirectory = Server.MapPath("~/Language/Recovery/");

                var pageSourcePath = sourcedirectory + fileName;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pageSourcePath))
                {
                    file.Write(json);
                }

                staticLoad();
                /*Global.json güncelleniyor*/
                var texts = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
                foreach (var item in texts)
                {
                    if (Global.Count(a => a.Key == item.Key) == 0)
                    {
                        Global.Add(item.Key, item.Value);
                    }
                    else
                    {
                        foreach (var item2 in item.Value)
                        {
                            if (Global[item.Key].Count(a => a.Key == item2.Key) == 0)
                            {
                                Global[item.Key].Add(item2.Key, item2.Value);
                            }
                            else
                            {
                                Global[item.Key][item2.Key] = item2.Value;
                            }
                        }
                    }
                }

                var globalSourcePath = sourcedirectory + "global.json";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(globalSourcePath))
                {
                    var globaljson = Infoline.Helper.Json.Serialize(Global);
                    file.Write(globaljson);
                }

                /*Global.json güncelleniyor*/


                ///*Yedek*/
                if (System.IO.File.Exists(pageSourcePath))
                {
                    System.IO.File.Copy(pageSourcePath, recoverydirectory + fileName.Substring(0, fileName.Length - 5) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".json");
                }

                if (System.IO.File.Exists(globalSourcePath))
                {
                    System.IO.File.Copy(globalSourcePath, recoverydirectory + "global_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".json");
                }
                ///*Yedek*/


                return Json(true, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }








        [AllowEveryone]
        public ContentResult Load()
        {
            var jsonFile = BusinessExtensions.GetStringFromUrl("https://10.100.0.222/language/sources/global.json");

            if (!String.IsNullOrEmpty(jsonFile))
            {

                var xx = HttpContext.Server.MapPath("/language/sources/global.json");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(xx))
                {
                    var globaljson = jsonFile;
                    file.Write(globaljson);
                }

            }

            return Content("true");

        }

        [AllowEveryone]
        public ContentResult MergeFiles()
        {

            var jsonFile1 = BusinessExtensions.GetStringFromUrl("https://havakalitesi.ibb.istanbul/language/sources/global.json");
            var jsonFile2 = BusinessExtensions.GetStringFromUrl("http://sim.csb.gov.tr/language/sources/global.json");

            var ibb = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonFile1);
            var sim = Infoline.Helper.Json.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonFile2);

            var difSim = sim.Where(a => !ibb.Select(b => b.Key).Contains(a.Key)).ToArray();
            var difIbb = ibb.Where(a => !sim.Select(b => b.Key).Contains(a.Key)).ToArray();

            var commonItems = sim.Where(a => ibb.Select(b => b.Key).Contains(a.Key)).ToArray();

            foreach (var item in commonItems)
            {

                var common = ibb.Where(a => a.Key == item.Key).FirstOrDefault();
                var notInSim = common.Value.Where(a => !item.Value.Select(b => b.Key).Contains(a.Key)).ToArray();

                if (notInSim.Count() > 0)
                {
                    foreach (var notsim in notInSim)
                    {
                        item.Value.Add(notsim.Key, notsim.Value);
                    }
                }

            }

            foreach (var dibb in difIbb)
            {
                sim.Add(dibb.Key, dibb.Value);
            }

            return Content(Infoline.Helper.Json.Serialize(sim), "text/html");

        }




    }
}