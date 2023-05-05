namespace Final_Project_Tenslog.Models
{
    public class MyDirect
    {
        public int Id { get; set; }

        public AppUser WriteingWithUser { get; set; }

        public string WriteingWithUserId { get; set; }

        public AppUser AppUser { get; set; }

        public string AppUserId { get; set; }

        public List<Message> Messages { get; set; }
    }
}
