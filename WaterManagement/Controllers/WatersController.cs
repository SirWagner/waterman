﻿using System;
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

namespace Waters.Controllers
{
    public class WatersController : Controller
    {
        private readonly WaterDbContext _db;
        [BindProperty]
        public Clients Clients { get; set; }
        public WatersController(WaterDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            var Index = _db.Clients.ToList();
            return View(Index);
        }

        public IActionResult Index2()
        {
            var Index = _db.Clients.ToList();
            return View(Index);
        }


        public IActionResult Search()
        {
            return View();
        }

        public IActionResult Pendente()
        {
            var Pendente = _db.Clients.Where(c => !string.IsNullOrEmpty(c.Bill) && c.Payed == null).ToList();
            return View(Pendente);
        }

        public IActionResult Atrasado()
        {
            var Atrasado = _db.Clients.Where(c => !string.IsNullOrEmpty(c.Bill) && c.Payed == null && DateTime.Now > c.PayDate).ToList();
            return View(Atrasado);
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


        public IActionResult Create(string? Meter, int? id)
        {
            Clients = new Clients();
            //if (id == null)
            // {
            
            Clients.Meter = Meter;//id eh codideia
            var join = _db.User.Where(x => x.Meter == Clients.Meter).FirstOrDefault();

            Clients.Name = join.Nome;
            Clients.State = join.State;
            Clients.Meter = join.Meter;
            Clients.Celular = join.Celular;
            Clients.Email = join.Email;
            Clients.Address = join.Address;

            int lastId = _db.Clients.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefault(); // Get the last created ID
            int newId = lastId + 1; // Increment to get the next ID
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Generates a random 4-digit number
            Clients.Bill = $"FJ{randomNumber}{newId}";

            DateTime today = Clients.Created;
            DateTime nextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
            DateTime payDate = new DateTime(nextMonth.Year, nextMonth.Month, 10);

            Clients.PayDate = payDate;



            //create
            return View(Clients);
        }

        public IActionResult CreateU(string? Meter, int? id)
        {
            Clients = new Clients();
            //if (id == null)
            // {

            Clients.Meter = Meter;//id eh codideia
            var join = _db.User.Where(x => x.Meter == Clients.Meter).FirstOrDefault();

            Clients.Name = join.Nome;
            Clients.State = join.State;
            Clients.Meter = join.Meter;
            Clients.Celular = join.Celular;
            Clients.Email = join.Email;
            Clients.Address = join.Address;

            int lastId = _db.Clients.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefault(); // Get the last created ID
            int newId = lastId + 1; // Increment to get the next ID
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Generates a random 4-digit number
            Clients.Bill = $"Outros Custos{newId}";

            Clients.PayDate = DateTime.Now;



            //create
            return View(Clients);
        }


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


        public IActionResult Pay(int? id, string meter)
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

        public IActionResult Details2(int? id)
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
            //return RedirectToAction("PrintInvoice", new { id = id });
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get the meter value associated with the given id
            var meterValue = _db.Clients
                .Where(c => c.Id == id)
                .Select(c => c.Meter)
                .FirstOrDefault();

            // Initialize model
            var model = new Atrasadas
            {
                Clients = new Clients(),
                ClientsList = _db.Clients
                    .Where(x => x.Meter == meterValue && x.Recibo == null)
                    .ToList()
            };

            // Fetch client details
            model.Clients = _db.Clients.FirstOrDefault(u => u.Id == id);
            if (model.Clients == null)
            {
                return NotFound();
            }

            // Check if PayDate is overdue and not yet paid
            if (model.Clients.PayDate < DateTime.Now && model.Clients.Payed == null)
            {
                model.Clients.Multa = model.Clients.Valor * 0.25m;
                model.Clients.Debt = model.Clients.Valor + model.Clients.Multa;
                _db.Clients.Update(model.Clients);
                _db.SaveChanges();
            }

            // Calculate monthly consumption
            ViewBag.ConsumoMensal = model.Clients.M4 - model.Clients.M3;

            var billsStartingWithO = model.ClientsList?.Where(o => o.Bill != null && o.Bill.StartsWith("O"));

            if (billsStartingWithO != null && billsStartingWithO.Any())
            {
                ViewBag.OuC = billsStartingWithO.Sum(o => o.Debt);
            }
            // Calculate GrandTotal safely (handling null Debt values)
            ViewBag.GrandTotal = model.ClientsList?.Sum(c => c.Debt ?? 0) ?? 0;

            return View(model);
        }




        public IActionResult Print(int? id)
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
            //return View(Clients);
            return RedirectToAction("PrintInvoice", new { id = id });
        }

        public IActionResult PrintInvoice(int? id)
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
            return View();
            //return RedirectToAction("PrintInvoice", new { id = id });
        }

        



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(Search selectclient)
        {

            if (!ModelState.IsValid)
            {
                return View(nameof(Search));
            }
            IList<Clients>? lista = null;

            if (selectclient.Name == null && selectclient.Bill == null && selectclient.Meter == null && selectclient.DataInicio == null && selectclient.DataFim == null)
            {
                lista = _db.Clients.ToList();
            }
            else
             if (selectclient.Meter != null && selectclient.DataInicio == null && selectclient.DataFim == null)

            {
                lista = _db.Clients.Where(x =>  x.Meter == selectclient.Meter  ).ToList();
            }
            else
      
            {
                lista = _db.Clients.Where(x =>  x.Stablished >= selectclient.DataInicio && x.Stablished <= selectclient.DataFim && x.Meter == selectclient.Meter).ToList();
            }

            return View("Index2", lista);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string? meter)
        {
            Clients.Debt = Clients.Payed - Clients.Multa + Clients.Valor;

            if (ModelState.IsValid)
            {
                if (Clients.Id == 0)
                {
                    //create
                    _db.Clients.Add(Clients);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Clients.Meter))
                    {
                        Clients.Debt = Clients.Payed - Clients.Multa + Clients.Valor;
                    }

                    _db.Clients.Update(Clients);
                }

              
                

                var join = _db.User.Where(x => x.Meter== Clients.Meter).FirstOrDefault();

                Clients.Name = join.Nome;
                Clients.Meter = join.Meter;
                Clients.Celular = join.Celular;
                Clients.Email = join.Email;
                Clients.Address = join.Address;


                int v = _db.SaveChanges();

                return RedirectToAction("Water", "users", new {meter = meter});
            }
            return View(Clients);
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
                        
                        var valor = (Clients.M4 - Clients.M3) * 70;
                        if (valor <= 350)
                        {
                            Clients.Valor = 350;
                        }
                        else
                        {

                            Clients.Valor = valor;
                        }

                        

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
        public IActionResult CreateU(Clients clients, string? meter)
        {
            if (ModelState.IsValid)
            {
                // Verificar se o campo Data de Emissão está preenchido corretamente
                if (clients.Created == default)
                {
                    ModelState.AddModelError("Created", "A Data de Emissão é obrigatória.");
                    return View(clients);
                }

                if (clients.Id == 0) // Se for um novo cliente
                {
                    if (!string.IsNullOrEmpty(clients.Meter))
                    {
                       

                        var join = _db.User.FirstOrDefault(x => x.Meter == clients.Meter);

                        if (join != null)
                        {
                            clients.Name = join.Nome;
                            clients.Meter = join.Meter;
                            clients.Celular = join.Celular;
                            clients.Email = join.Email;
                        }

                       clients.Debt = clients.Valor;
                        clients.PayDate = clients.Created;
                        
                    }

                    // Adicionar o cliente
                    _db.Clients.Add(clients);
                }
                else
                {
                    _db.Clients.Update(clients);
                }

                _db.SaveChanges();

                TempData["SuccessMessage"] = "Fatura adicionada com sucesso!";

                return RedirectToAction("Water", "users", new { meter = meter }); // Redireciona após salvar
            }

            return View(clients); // Retorna a mesma view se ModelState não for válido
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
