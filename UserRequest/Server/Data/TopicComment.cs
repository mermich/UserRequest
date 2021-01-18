using UserRequest.Server.Models;

namespace UserRequest.Server.Data
{
    public class TopicComment
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public ApplicationUser Author { get; set; }
    }
}
