using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace WaterManagement.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly WaterDbContext _db;
        [BindProperty]
        public Pay Pay { get; set; }
        public Clients Clients { get; set; }
        public PaymentsController(WaterDbContext db)
        {
            _db = db;
        }



        public IActionResult Index()
        {
            var Index = _db.Pay.ToList();
            return View(Index);
        }

        public IActionResult ToPay(int? id)
        {
            Clients = new Clients();

            // For a new client (id == null)
            if (id == null)
            {
                // Generate Recibo for the new client
                var lastClient = _db.Clients.OrderByDescending(c => c.Id).FirstOrDefault();
                int nextId = (lastClient?.Id + 1) ?? 1;
                string randomPart = new Random().Next(1000, 9999).ToString();
                Clients.Recibo = $"RJ{randomPart}{nextId}";

              
               

                return View(Clients);
            }

            // For updating an existing client
            Clients = _db.Clients.FirstOrDefault(u => u.Id == id);
            if (Clients == null)
            {
                return NotFound();
            }

            // Generate Recibo for the existing client if not already set
            if (string.IsNullOrEmpty(Clients.Recibo))
            {
                string randomPart = new Random().Next(1000, 9999).ToString();
                Clients.Recibo = $"RJ{randomPart}{Clients.Id}";

                Clients.ReDate = DateTime.Now;
            }

            // Check if the current date is greater than PayDate
            if (DateTime.Now > Clients.PayDate)
            {
                // Calculate Multa (25% of Valor) and Debt
                Clients.Multa = Clients.Valor * 0.25m; // Assuming Valor is a decimal
                Clients.Debt = Clients.Multa + Clients.Valor;
            }
            else
            {
                // If no penalty, Debt equals Valor
                Clients.Debt = Clients.Valor;
            }

            return View(Clients);
        }









        public IActionResult Upsert(string? meter)
        {
            Pay = new Pay();
            //if (id == null)
           // {
                
                Pay.Meter = meter;//id eh codideia


            //create
            return View(Pay);
            //}
            //update
            /*Coment = _db.Coment.FirstOrDefault(u => u.Id == id);
            if (Coment == null)

            {

                return NotFound();
            }*/
            //return View(Coment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToPay(Clients model)
        {
            if (ModelState.IsValid)
            {
                var client = _db.Clients.FirstOrDefault(c => c.Id == model.Id);
                if (client != null)
                {
                    // Update properties
                    client.Payed = model.Payed;
                    client.PayDate = model.PayDate;
                    client.Banco = model.Banco;
                    client.Recibo = model.Recibo; // Use model.Recibo here

                    // Apply penalty if applicable
                    if (client.ReDate > model.PayDate)
                    {
                        client.Multa = model.Valor * 0.25m;
                        client.Debt = client.Multa + model.Valor;
                    }
                    else
                    {
                        client.Debt = model.Valor;
                    }

                    _db.Clients.Update(client);
                    _db.SaveChanges();
                }

                TempData["SuccessMessage"] = "Pagamento registado com sucesso!";
                return RedirectToAction("Index", "Users"); ;
            }

            return View(model);


        }

        public IActionResult Recibo(int? id)
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
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Pay.Id == 0)
                {
                    //create
                    //_db.Coment.Add(Coment);
                }
                else
                {
                    //_db.Coment.Update(Coment);
                }
                int v = _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Pay);
        }






    }
}