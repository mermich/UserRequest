using System;
using System.Collections.Generic;
using System.Text;

namespace UserRequest.Shared
{
    public class Topic
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Votes { get; set; }

        public int Comments { get; set; }
    }
}
