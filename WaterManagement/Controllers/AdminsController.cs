using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rotativa.AspNetCore;
using System.Data.Entity;
using System.ComponentModel;


namespace Admins.Controllers
{
  
    public class AdminsController : Controller
    {
        private readonly WaterDbContext _db;
        [BindProperty]
        public Clients Clients { get; set; }
        public User User { get; set; }
        public AdminsController(WaterDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            var Index = _db.Clients.ToList();
            return View(Index);
        }

        public IActionResult Facturas(string meter)
        {
            var Facturas = _db.Clients.Where(x => x.Meter == meter).ToList();
            return View(Facturas);
        }


        public IActionResult Users()
        {
            var Index = _db.User.ToList();
            return View(Index);
        }


        public IActionResult Invoice()
        {
            return View();
        }


    

        public IActionResult Pay(int Id)
        {

            var meter = _db.Pay.Where(x => x.Id == Id).ToList();

            return View(meter);
        }

        //[HttpPost]
        //public IActionResult DownloadExcel()
        //{
        //    // Set EPPlus license context for non-commercial use
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Clients");

        //        // Load data from Clients list to worksheet
        //        var clients = _db.Clients.ToList();
        //        worksheet.Cells["A1"].LoadFromCollection(clients, true, TableStyles.Medium9);

        //        // Auto-fit columns for better appearance
        //        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        //        // Prepare the Excel file for download
        //        var content = package.GetAsByteArray();
        //        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        var fileName = "ClientsData.xlsx";

        //        return File(content, contentType, fileName);
        //    }
        //}



        public IActionResult Upsert(int? id, string meter)
        {
            Clients = new Clients();

            if (id == null)
            {
                // Use the passed Meter field
                Clients.Meter = meter;

                // Generate Recibo for the new client
                var lastClient = _db.Clients.OrderByDescending(c => c.Id).FirstOrDefault();
                int nextId = (lastClient?.Id + 1) ?? 1;
                string randomPart = new Random().Next(1000, 9999).ToString();
                Clients.Recibo = $"RJ{randomPart}{nextId}";

                return View(Clients);
            }

            // For updating existing client
            Clients = _db.Clients.FirstOrDefault(u => u.Id == id);
            if (Clients == null)
            {
                return NotFound();
            }

            return View(Clients);
        }




        public IActionResult Edit(int? id)
        {
            Clients = new Clients();
            if (id == null)
            {
                //create
                return View(Clients);
            }
            //update
            Clients = _db.Clients.FirstOrDefault(u => u.Id == id);
            if (Clients == null)
            {
                return NotFound();
            }
            return View(Clients);
        }


        public IActionResult EditU(int? id)
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
        public IActionResult EditU(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id == 0)
                {
                    // Create new user
                    _db.User.Add(user);
                }
                else
                {
                    // Update existing user
                    _db.User.Update(user);
                }
                _db.SaveChanges();
                return RedirectToAction("Users");
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Clients clients)
        {
            if (ModelState.IsValid)
            {
                if (clients.Id == 0)
                {
                    // Create new user
                    _db.Clients.Add(clients);
                }
                else
                {
                    // Update existing user
                    _db.Clients.Update(clients);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clients);
        }


        public IActionResult Details(int? id)
        {
            Clients = new Clients();

            if (id == null)
            {
                // Create new client
                return View(Clients);
            }

            // Retrieve existing client for update
            Clients = _db.Clients.FirstOrDefault(u => u.Id == id);
            if (Clients == null)
            {
                return NotFound();
            }

            // Check if PayDate is due and Payed is null
            if (Clients.PayDate < DateTime.Now && Clients.Payed == null)
            {
                // Calculate Multa (25% of Valor)
                Clients.Multa = Clients.Valor * 0.25m;

                // Calculate total Debt (Valor + Multa)
                Clients.Debt = Clients.Valor + Clients.Multa;

                // Save changes to the database
                _db.Clients.Update(Clients);
                _db.SaveChanges();
            }

            return View(Clients);
        }




        [HttpPost]
        public IActionResult DeleteItemU(int id)
        {
            try
            {
                var recordsToDelete = _db.User.Where(x => x.Id == id).ToList();

                if (recordsToDelete.Count > 0)
                {
                    _db.User.RemoveRange(recordsToDelete);
                    _db.SaveChanges();
                    TempData["SuccessMessage"] = "Cliente apagado com sucesso";
                    return RedirectToAction("Users");
                }
                else
                {
                    TempData["ErrorMessage"] = "No user found to delete.";
                    return RedirectToAction("Users");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user: " + ex.Message;
                return RedirectToAction("Users");
            }
        }


        public IActionResult DeleteItem(int id)
        {
            try
            {
                var recordsToDelete = _db.Clients.Where(x => x.Id == id).ToList();

                if (recordsToDelete.Count > 0)
                {
                    _db.Clients.RemoveRange(recordsToDelete);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return NoContent(); // Return 204 No Content when there are no records to delete.
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting records from TbDirCoord: " + ex.Message);
            }
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string? meter)
        {
            if (ModelState.IsValid)
            {
                // Verificar se o campo Data de Emissão está preenchido
                if (Clients.Created == default)
                {
                    // Retorna a mesma view sem gravar, com validação feita no frontend
                    return View(Clients);
                }

                if (Clients.Id == 0)
                {
                    if (!string.IsNullOrEmpty(Clients.Meter))
                    {
                        Clients.Valor = (Clients.M4 - Clients.M3) * 70;
                        Clients.Debt = (Clients.Multa ?? 0) + Clients.Valor; 

                        var join = _db.User.Where(x => x.Meter == Clients.Meter).FirstOrDefault();

                        if (join != null)
                        {
                            Clients.Name = join.Nome;
                            Clients.Meter = join.Meter;
                            Clients.Celular = join.Celular;
                            Clients.Email = join.Email;
                        }

                        Random random = new Random();
                        int randomNumber = random.Next(1000, 9999); // Generates a random 4-digit number
                        Clients.Bill = $"FJ{randomNumber}{Clients.Id}";

                        DateTime today = Clients.Created;
                        DateTime nextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
                        DateTime payDate = new DateTime(nextMonth.Year, nextMonth.Month, 10);

                        Clients.PayDate = payDate;
                    }

                    // Adicionar o cliente
                    _db.Clients.Add(Clients);
                }
                else
                {
                    _db.Clients.Update(Clients);
                }

                _db.SaveChanges();

                TempData["SuccessMessage"] = "Fatura adicionada com sucesso!";

                return RedirectToAction("Water", "users", new { meter = meter }); // Redireciona após salvar
            }

            return View(Clients); // Retorna a mesma view se ModelState não for válido
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(string? meter, string recibo)
        {
            if (ModelState.IsValid)
            {

                if (Clients.Id == 0)
                {
                    if (!string.IsNullOrEmpty(Clients.Meter))
                    {

                        Clients.Debt = Clients.Valor - Clients.Payed;

                        var join = _db.User.Where(x => x.Meter == Clients.Meter).FirstOrDefault();

                        Clients.Name = join.Nome;
                        Clients.Meter = join.Meter;
                        Clients.Celular = join.Celular;
                        Clients.Email = join.Email;
                        Clients.Address = join.Address;
                        Clients.State = join.State;
                        Clients.Recibo = recibo;
                        Clients.Created = DateTime.Now;

                        Random random = new Random();
                        int randomNumber = random.Next(1000, 9999); // Generates a random 4-digit number
                        Clients.Bill = $"FJ{randomNumber}{Clients.Id}";

                    }

                    //create
                    _db.Clients.Add(Clients);
                }
                else
                {
                    _db.Clients.Update(Clients);
                }
                int v = _db.SaveChanges();
                return RedirectToAction("Water",  "users", new { meter = meter });
            }
            return View(Clients);
        }


    }

    
}
