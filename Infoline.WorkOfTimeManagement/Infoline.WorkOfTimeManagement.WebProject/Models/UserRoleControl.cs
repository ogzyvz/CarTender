using HtmlAgilityPack;
using Infoline.Framework.Database;
using Infoline.WorkOfTimeManagement.BusinessAccess;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace System.Web.Mvc
{

    public class AllowEveryone : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            return;
        }
    }

    public sealed class UserRoleHtmlControl : ActionFilterAttribute
    {

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            if (httpContext.Request.IsAjaxRequest() || httpContext.Response.ContentType != "text/html")
            {
                base.OnResultExecuted(filterContext);
                return;
            }

            if (httpContext.Session == null || httpContext.Session["userStatus"] == null)
            {
                base.OnResultExecuted(filterContext);
                return;
            }


            var originalFilter = httpContext.Response.Filter;
            var pageSecurity = (PageSecurity)httpContext.Session["userStatus"];


            if (originalFilter == null || pageSecurity == null || pageSecurity.PagesRoles == null)
            {
                base.OnResultExecuted(filterContext);
                return;
            }

            filterContext.HttpContext.Response.Filter = new KeywordStream(originalFilter, pageSecurity);
            base.OnResultExecuted(filterContext);
        }

        public class KeywordStream : MemoryStream
        {
            StreamWriter stream;
            public PageSecurity pageSecurity;
            public string[] pageList;
            public string[] attributes = new string[] { "href", "data-href", "data-url" /*"src","url",*/  };


            public KeywordStream(Stream s, PageSecurity pSecurity)
            {
                stream = new StreamWriter(s);
                pageSecurity = pSecurity;
                pageList = pSecurity != null ? pSecurity.PagesRoles.Where(a => a.status == false).Select(a => a.Action_Title).ToArray() : new string[] { };
            }


            public override void Close()
            {

                byte[] allBytes = this.ToArray();
                string source = System.Text.Encoding.UTF8.GetString(allBytes);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(source);

                List<HtmlNode> nodes = document.DocumentNode.Descendants().ToList();

                for (int i = 0; i < nodes.Count(); i++)
                {
                    var node = nodes[i];
                    for (int ai = 0; ai < attributes.Length; ai++)
                    {
                        var attribute = attributes[ai];
                        if (node.Name != "link" && node.Attributes.Contains(attribute))
                        {
                            var value = node.Attributes[attribute].Value;
                            if (!string.IsNullOrEmpty(value))
                            {
                                foreach (var item in pageList)
                                {
                                    var basevalue = value.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries).First();
                                    var basevalue2 = basevalue;

                                    var pathLArr = basevalue.Split('/');

                                    if (pathLArr.Length == 2)
                                    {
                                        basevalue2 = basevalue + "/Index";
                                    }

                                    if (pathLArr.Length == 3)
                                    {
                                        basevalue2 = basevalue + "/Index";
                                    }

                                    if (basevalue == item || basevalue2 == item)
                                    {
                                        nodeDetected(node).Remove();
                                    };
                                }
                            }
                        }
                    }
                }


                var menu = nodes.Where(a => a.Name == "ul" && a.Attributes.Contains("id") && a.Attributes["id"].Value == "side-menu").FirstOrDefault();
                if (menu != null)
                {
                    var li = menu.Descendants().Where(a => a.Name == "li").Select(a => new
                    {
                        node = a,
                        count = a.Descendants().Count(b =>
                        b.Name == "a" && b.Attributes.Contains("href") && (b.Attributes["href"].Value != "#" && b.Attributes["href"].Value != ""))
                    }).Where(a => a.count == 0).ToArray();


                    foreach (var item in li)
                    {
                        item.node.Remove();
                    }
                }


                stream.Write(document.DocumentNode.OuterHtml);
                stream.Flush();
                stream.Close();
                base.Close();
            }



            public HtmlNode nodeDetected(HtmlNode node)
            {
                if (node.ParentNode == null)
                {
                    return node;
                }

                if (node.ParentNode.ChildNodes.Where(a => a.NodeType != HtmlNodeType.Text).Count() == 1)
                {
                    return nodeDetected(node.ParentNode);
                }
                else
                {
                    return node;
                }
            }
        }

    }

    public sealed class UserRoleControl : AuthorizeAttribute
    {
        public static Dictionary<string, string> CustomAttr = new Dictionary<string, string>();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var httpContext = filterContext.RequestContext.HttpContext;

            //  AllowEveryone Tanımlanmamış ise
            if (!(filterContext.ActionDescriptor.IsDefined(typeof(AllowEveryone), true)
               || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowEveryone), true)))
            {

                if (httpContext.Session == null || (httpContext.Session["ticketid"] == null || httpContext.Session["userStatus"] == null))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new ContentResult
                        {
                            Content = Infoline.Helper.Json.Serialize(new ResultStatus
                            {
                                objects = new FeedBack().Warning("Kullanıcı girişi yapmanız gerekiyor.", false, "/Account/SignIn"),
                                message = "UserControl",
                                result = true,
                            }),
                            ContentType = "application/json",
                            ContentEncoding = Text.Encoding.UTF8,
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "action", "SignIn" },
                            { "controller", "Account" },
                            { "area", String.Empty },
                            { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                        });
                    }
                    return;
                }








                if (!PageValidator(filterContext))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new ContentResult
                        {
                            Content = Infoline.Helper.Json.Serialize(new ResultStatus
                            {
                                objects = new FeedBack().Warning("Yapmak istediğiniz istek için tarafınıza yetki verilmemiş ! Yöneticiniz ile görüşün.", false, "/Account/SignIn"),
                                message = "UserControl",
                                result = true,
                            }),
                            ContentType = "application/json",
                            ContentEncoding = Text.Encoding.UTF8,
                        };
                    }
                    else
                    {
                        if (filterContext.IsChildAction == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                            {
                                { "action", "AccessDenied" },
                                { "controller", "Error" },
                                { "area",String.Empty },
                                { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                            });

                        }
                    }




                    return;
                }



            }
        }

        private bool PageValidator(AuthorizationContext filterContext)
        {
            if (filterContext.RouteData.Values["controller"] == null)
            {
                return false;
            }

            var httpContext = filterContext.RequestContext.HttpContext;
            var area = filterContext.RouteData.DataTokens["area"] != null ? "/" + filterContext.RouteData.DataTokens["area"].ToString() : "";
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();
            var requestPage = string.Format("{0}/{1}/{2}", area, controller, action);

            if (httpContext.Session == null || httpContext.Session["userStatus"] == null)
            {
                return false;
            }

            var userStatus = (PageSecurity)filterContext.RequestContext.HttpContext.Session["userStatus"];

            if (userStatus == null || userStatus.PagesRoles == null)
            {
                return false;
            }


            return userStatus.PagesRoles.Count(a => a.status == true && !String.IsNullOrEmpty(a.Action_Title) && (a.Action_Title == requestPage || a.Action_Title.ToUpper(new CultureInfo("en-US", false)) == requestPage.ToUpper(new CultureInfo("en-US", false)))) > 0;

        }

    }
}