
using LiteDB;
using System;
using System.Collections.Generic;

namespace kwisatz.Data.Entities
{
    public class JournalTag
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}