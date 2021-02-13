using System;

namespace UserRequest.Shared
{
    public class TopicComment
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public string Author { get; set; }

        public DateTime Created { get; set; }
    }
}