using UserRequest.Server.Models;

namespace UserRequest.Server.Data
{
    public class TopicVote
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public Topic Topic { get; set; }
    }
}
