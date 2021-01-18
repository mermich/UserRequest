using System;
using System.Collections;
using System.Collections.Generic;
using UserRequest.Server.Models;

namespace UserRequest.Server.Data
{
    public class Topic
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public ApplicationUser Author { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<TopicComment> TopicComments { get; set; }

        public IEnumerable<TopicVote> Votes { get; set; }

    }
}
