namespace Final_Project_Tenslog.Models
{
    public class Follower
    {
        public AppUser User { get; set; }

        public string UserId { get; set; }

        public AppUser UserFollower { get; set; }

        public string UserFollowerId { get; set; }
    }
}
