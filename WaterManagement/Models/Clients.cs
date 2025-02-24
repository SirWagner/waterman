using System;
using System.Collections.Generic;

namespace WaterManagement.Models;

public partial class Clients
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? State { get; set; }

    public string? Meter { get; set; }

    public string? Bill { get; set; }

    public string? Celular { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Banco { get; set; }

    public string? Recibo { get; set; }

    public decimal? M3 { get; set; }
    public decimal? M4 { get; set; }

    public decimal? Valor { get; set; }

    public decimal? Payed { get; set; }

    public decimal? Multa { get; set; }

    public decimal? Debt { get; set; }

    public DateTime PayDate { get; set; }

    public DateTime ReDate { get; set; }

    public DateTime Created { get; set; }

    public DateTime Stablished { get; set; }

    public byte[]? Picture { get; set; }

    public string? Coment { get; set; }
}
