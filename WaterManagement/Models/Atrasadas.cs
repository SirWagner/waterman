namespace WaterManagement.Models
{
    public class Atrasadas
    {
        public Clients Clients { get; set; } // Single client details
        public List<Clients> ClientsList { get; set; } // List of all clients
    }
}
