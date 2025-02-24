using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WaterManagement.Models;

namespace WaterManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WaterDbContext _db;

        public HomeController(ILogger<HomeController> logger, WaterDbContext db)
        {
            _logger = logger;
            _db = db;
        }
               
        [BindProperty]
        public Clients Clients { get; set; }
       
        public IActionResult Index()
        {
            ViewBag.TotalNames = _db.Clients.Select(c => c.Name).Distinct().Count();

            // Count of Faturas
            ViewBag.TotalFaturas = _db.Clients.Count(c => !string.IsNullOrEmpty(c.Bill));


            ViewBag.TotalPendente = _db.Clients.Count(c => !string.IsNullOrEmpty(c.Bill) && c.Payed == null);

            ViewBag.TotalAtrasado = _db.Clients.Count(c => !string.IsNullOrEmpty(c.Bill) && c.Payed == null && DateTime.Now > c.PayDate);


            // Sum of Valor
            ViewBag.TotalValor = _db.Clients.Sum(c => c.Valor) ?? 0;

            // Sum of Payed
            ViewBag.TotalPayed = _db.Clients.Sum(c => c.Payed) ?? 0;


            return View();
        }

                
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
