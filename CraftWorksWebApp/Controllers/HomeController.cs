using Microsoft.AspNetCore.Mvc;
using CraftWorksWebApp.Models;
using System.Collections.Generic;

namespace CraftWorksWebApp.Controllers
{
    public class CraftWorksController : Controller
    {
        private readonly DataAccess _dataAccess;

        public CraftWorksController(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            List<Product> products = _dataAccess.GetAllProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult PlaceOrder(int productId)
        {
            // For demonstration purposes, let's assume the user ID is 1
            int userId = 1;
            _dataAccess.PlaceOrder(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Orders()
        {
            // For demonstration purposes, let's assume the user ID is 1
            int userId = 1;
            List<Transaction> orders = _dataAccess.GetUserOrders(userId);
            return View(orders);
        }
    }
}

namespace CraftWorksWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.MissionStatement = "Crafting memories one creation at a time.";
            return View();
        }
    }
}

namespace CraftWorksWebApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

namespace CraftWorksWebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}



