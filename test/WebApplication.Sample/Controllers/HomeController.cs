using Microsoft.AspNetCore.Mvc;
using QbSync.WebConnector.Core;
using System;
using System.Text;

namespace WebApplication.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebConnectorQwc webConnectorQwc;

        public HomeController(
            IWebConnectorQwc webConnectorQwc
        )
        {
            this.webConnectorQwc = webConnectorQwc;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public FileResult DownloadQwc()
        {
            var fileName = $"my.qwc";

            var url = Request.Scheme + System.Uri.SchemeDelimiter + Request.Host;
            var data = webConnectorQwc.GetQwcFile(new QbSync.WebConnector.Models.WebConnectorQwcModel(
                appName: "My App",
                appDescription: "Sync QuickBooks with My Website",
                appSupport: $"{url}/support",
                appURL: $"{url}/QBConnectorAsync.asmx",
                fileID: Guid.NewGuid(), // Don't generate a new guid all the time, save it somewhere
                ownerID: Guid.NewGuid(), // Don't generate a new guid all the time, save it somewhere
                userName: "jsgoupil",
                runEvery: new TimeSpan(0, 30, 0),
                qbType: QbSync.WebConnector.Models.QBType.QBFS
            ));

            byte[] fileBytes = Encoding.ASCII.GetBytes(data);
            return File(fileBytes, "application/download", fileName);
        }
    }
}