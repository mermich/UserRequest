using System;
using UserRequest.Server.Models;

namespace UserRequest.Server.Data
{
    public class TopicComment
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public Topic Topic { get; set; }
        public int TopicId { get; set; }

        public DateTime Created { get; set; }

        public ApplicationUser Author { get; set; }
        public string AuthorId { get; set; }
    }
}
