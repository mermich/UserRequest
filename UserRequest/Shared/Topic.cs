using System;
using System.Collections;
using System.Collections.Generic;

namespace UserRequest.Shared
{
    public class Topic
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Votes { get; set; }

        public int CommentsCount { get; set; }
        public IEnumerable<TopicComment> Comments { get; set; }

        public string Author { get; set; }

        public DateTime Created { get; set; }       
    }
}
