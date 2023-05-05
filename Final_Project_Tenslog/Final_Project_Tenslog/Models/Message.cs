namespace Final_Project_Tenslog.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string MessageContent { get; set; }

        public MyDirect MyDirect { get; set; }

        public int MyDirectId { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        public string WhoWrite { get; set; }

    }
}
