using System;
using System.Collections.Generic;

namespace WaterManagement.Models;

public partial class Pay
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Email { get; set; }

    public string? Banco { get; set; }

    public string? Recibo { get; set; }

    public string? Data { get; set; }

    public string? Valor { get; set; }

    public string? Meter { get; set; }
}
