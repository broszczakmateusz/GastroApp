namespace GastroApp.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Table> Tables { get; set; } = new List<Table>();
    }
}
