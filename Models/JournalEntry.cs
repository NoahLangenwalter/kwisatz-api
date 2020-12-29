using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using kwisatz.Data.Entities;

namespace kwisatz.Models
{
    public class JournalEntrySubmission
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(1024)]
        public string Body { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

#nullable enable
    public class JournalEntryUpdate
    {
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(1024)]
        public string? Body { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }
#nullable disable

    public class JournalEntryModel
    {
        public string Id { get; }
        public string Title { get; }
        public string Body { get; }
        public DateTime CreatedDate { get; }
        public DateTime UpdatedDate { get; }
        public IEnumerable<string> Tags { get; }

        public JournalEntryModel(JournalEntry data)
        {
            Id = data.Id.ToString();
            Title = data.Title;
            Body = data.Body;
            Tags = data.Tags.Select(x => x.Name);
            CreatedDate = data.CreatedDate;
            UpdatedDate = data.LastUpdatedDate;
        }
    }
}