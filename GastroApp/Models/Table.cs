namespace GastroApp.Models
{
    public class Table
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }
        public bool IsTaken { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
