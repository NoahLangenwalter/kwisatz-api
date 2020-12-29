using LiteDB;
using System;
using System.Collections.Generic;

namespace kwisatz.Data.Entities
{
    public class JournalEntry
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<JournalTag> Tags { get; set; }

        public JournalEntry()
        {
        }

        public JournalEntry(string title, string body, List<JournalTag> tags)
        {
            Id = ObjectId.NewObjectId();
            CreatedDate = DateTime.Now;
            LastUpdatedDate = CreatedDate;
            Title = title;
            Body = body;
            Tags = tags;
        }

        public JournalEntry(string id, string title, string body, List<JournalTag> tags)
        {
            Id = new ObjectId(id);
            LastUpdatedDate = DateTime.Now;
            Title = title;
            Body = body;
            Tags = tags;
        }

        public void Merge(PartialJournalEntry partial)
        {
            LastUpdatedDate = partial.LastUpdatedDate;

            if(partial.Title != null)
                Title = partial.Title;
            if (partial.Body != null)
                Body = partial.Body;
            if (partial.Tags != null)
                Tags = partial.Tags;
        }
    }

#nullable enable
    public class PartialJournalEntry
    {
        public ObjectId Id { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public List<JournalTag>? Tags { get; set; }

        public PartialJournalEntry(string id, string title, string body, List<JournalTag> tags)
        {
            Id = new ObjectId(id);
            LastUpdatedDate = DateTime.Now;
            Title = title;
            Body = body;
            Tags = tags;
        }
    }
#nullable disable
}