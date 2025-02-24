using System;
using System.Collections.Generic;

namespace WaterManagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Celular { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Meter { get; set; }

    public string? State { get; set; }
}
