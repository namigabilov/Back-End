namespace Final_Project_Tenslog.Models
{
    public class Following
    {
        public AppUser User { get; set; }

        public string UserId { get; set; }

        public AppUser UserFollowing { get; set; }

        public string UserFollowingId { get; set; }
    }
}
