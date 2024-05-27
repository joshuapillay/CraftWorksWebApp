using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CraftWorksWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CraftWorksWebApp.Controllers
{
    //[Authorize]
    public class AdministratorController : Controller
    {
        private readonly DataAccess _dataAccess;

        public AdministratorController(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            List<Product> products = _dataAccess.GetAllProducts(); // Fetch products from the database
            return View(products); // Pass products to the view
        }

        [HttpPost]
        public IActionResult UpdatePrice(int productId, decimal newPrice)
        {
            _dataAccess.UpdateProductPrice(productId, newPrice);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddProduct(string name, decimal price, string category, bool availability, string imageUrl)
        {
            _dataAccess.InsertProduct(name, price, category, availability, imageUrl);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteProduct(string productName)
        {
            _dataAccess.DeleteProduct(productName);

            // Redirect back to the administrator page after deletion
            return RedirectToAction("Index");
        }
    }
}



