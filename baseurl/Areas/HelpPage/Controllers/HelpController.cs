using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using baseurl.Areas.HelpPage.ModelDescriptions;
using baseurl.Areas.HelpPage.Models;

namespace baseurl.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            var baseurl= Request.Url.GetLeftPart(UriPartial.Authority);
            var baseurl2 = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            var baseurl3 = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
            var baseurl4 = Url.Content("~");
            var baseurl5 = new Uri(Request.Url, Url.HttpRouteUrl("DefaultApi", "")).AbsoluteUri.ToString(); 

            ViewBag.BaseUrl = baseurl;//OK
            ViewBag.BaseUrl2 = baseurl2;//OK
            ViewBag.BaseUrl3= baseurl3;//OK
            ViewBag.BaseUrl4 = baseurl4;//NO
            ViewBag.BaseUrl5 = baseurl5;//NO

            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}