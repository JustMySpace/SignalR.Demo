using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SingalR.MVC.Server.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    ViewBag.id = string.Empty;

        //    return View(ViewBag);
        //}
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ViewBag.id = string.Empty;
            }
            else
            {
                ViewBag.id = id;
                try
                {
                    IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                    _context.Clients.Client(id).addMessage("用户扫码中", "等待确认信息");
                }
                catch (Exception)
                {
                }
            }

            return View(ViewBag);
        }

        [HttpPost]
        public void CheckIn(string id, string name)
        {
            try
            {
                IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
                _context.Clients.Client(id).addMessage("欢迎", name);
            }
            catch (Exception)
            {

            }
        }

    }
}