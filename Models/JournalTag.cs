using System;
using System.ComponentModel.DataAnnotations;
using kwisatz.Data.Entities;

namespace kwisatz.Models
{
    public class JournalTagModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public JournalTagModel(JournalTag data)
        {
            Id = data.Id;
            Name = data.Name;
        }
    }

    public class JournalTagSubmission
    {
        [Required]
        public string Name { get; set; }
    }
}