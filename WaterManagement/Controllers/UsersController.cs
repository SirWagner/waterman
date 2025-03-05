using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace WaterManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly WaterDbContext _db;
        [BindProperty]
        public User User { get; set; }
        public UsersController(WaterDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            var Index = _db.User.ToList();
            return View(Index);
        }

        public IActionResult Water(string Meter)
        {

            var meter = _db.Clients.Where(x => x.Meter == Meter).ToList();

            return View(meter);
        }

        public IActionResult Recibos(string Meter)
        {
            var meter = _db.Clients.Where(x => x.Meter == Meter && x.Recibo != null).ToList();

            if (!meter.Any())
            {
                TempData["ErrorMessage"] = "Nenhum recibo encontrado para o contador informado.";
                return RedirectToAction("Index"); // Redirect to another action
            }

            return View(meter);
        }


        public IActionResult Index2()
        {
            var Index = _db.User.ToList();
            return View(Index);
        }




        public IActionResult Search()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            User User = new User();

            if (id == null)
            {
                // Get the last user to increment the id
                var lastUser = _db.User.OrderByDescending(u => u.Id).FirstOrDefault();

                // Generate random 4-digit number
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);

                // Set the nº de contador
                string contador = $"J{randomNumber}{(lastUser?.Id + 1 ?? 1)}"; // If lastUser is null, start from 1

                User.Meter = contador;  // Set the contador in the new User object

                // Return the view for creating the user
                return View(User);
            }

            // Update case: retrieve the existing user
            User = _db.User.FirstOrDefault(u => u.Id == id);
            if (User == null)
            {
                return NotFound();
            }

            // Return the view for updating the user
            return View(User);
        }


        public IActionResult Details(int? id)
        {
            User = new User();
            if (id == null)
            {
                //create
                return View(User);
            }
            //update
            User = _db.User.FirstOrDefault(u => u.Id == id);
            if (User == null)
            {
                return NotFound();
            }
            return View(User);
        }

        public IActionResult Edit(int? id)
        {
            User = new User();
            if (id == null)
            {
                //create
                return View(User);
            }
            //update
            User = _db.User.FirstOrDefault(u => u.Id == id);
            if (User == null)
            {
                return NotFound();
            }
            return View(User);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SelUsers selectusers)
        {

            if (!ModelState.IsValid)
            {
                return View(nameof(Search));
            }
            IList<User>? lista = null;

            if (selectusers.Nome == null && selectusers.Meter == null )
            {
                lista = _db.User.ToList();
            }
            else
            if (selectusers.Nome != null && selectusers.Meter == null)
            {
                lista = _db.User.Where(x => x.Nome == selectusers.Nome ).ToList();
            }
            else
            if (selectusers.Nome == null && selectusers.Meter != null)
            {
                lista = _db.User.Where(x => x.Meter == selectusers.Meter).ToList();
            }
            else
            {
                lista = _db.User.Where(x => x.Meter == selectusers.Meter && x.Nome== selectusers.Nome).ToList();
            }

            return View("Index", lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (User.Id == 0)
                {
                    //create
                    _db.User.Add(User);
                }
                else
                {
                    _db.User.Update(User);
                }
                
                int v = _db.SaveChanges();
                return RedirectToAction("Upsert", "Waters", new { meter = User.Meter });
            }
            return View(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                if (User.Id == 0)
                {
                    //create
                    _db.User.Add(User);
                }
                else
                {
                    _db.User.Update(User);
                }
                int v = _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(User);
        }
    }
}






//    #region API Calls
//    [HttpGet]
//    public IActionResult GetAll()
//    {
//        return Json(new { data = _db.Ideias.ToList() });
//    }

//    [HttpDelete]
//    public IActionResult Delete(int id)
//    {
//        var ffFromDb = _db.Ideias.FirstOrDefault(u => u.Id == id);
//        if (ffFromDb == null)
//        {
//            return Json(new { success = false, message = "Error while Deleting" });
//        }
//        _db.Ideias.Remove(ffFromDb);
//        _db.SaveChanges();
//        return Json(new { success = true, message = "Delete successful" });
//    }
//    #endregion
//}
//}