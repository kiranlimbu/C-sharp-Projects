using GroceryStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GroceryStore.Models;

namespace GroceryStore.Controllers
{
    public class GroceryController : Controller
    {
        private IGroceryRepository _repository;

        public GroceryController(IGroceryRepository repository)
        {
            _repository = repository;
        }

        // GET: Grocery
        public ActionResult Index()
        {
            return View(_repository.GetItems());
        }

        // GET: Grocery/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Grocery/Create
        [HttpPost]
        public ActionResult Create(GroceryItem item)
        {
            _repository.AddItem(item);
            _repository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}