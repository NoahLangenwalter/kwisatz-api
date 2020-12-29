using System;
using System.Collections.Generic;
using kwisatz.Data.Entities;
using LiteDB;
using System.Linq;

namespace kwisatz.Data
{
    public class DataService : IDataService
    {
        private LiteDatabase _db;

        public DataService(ILiteDbContext dbContext)
        {
            _db = dbContext.Database;

            _db.Mapper.Entity<JournalEntry>()
                .Id(x => x.Id)
                .DbRef(x => x.Tags, "journalTag");

            _db.Mapper.Entity<JournalTag>()
                .Id(x => x.Id);
        }

        public IEnumerable<JournalEntry> GetEntries()
        {
            var entryCol = _db.GetCollection<JournalEntry>("journalEntry");
            entryCol.EnsureIndex(x => x.CreatedDate);

            var result = entryCol.Include(x => x.Tags).FindAll();
            return result;
        }

        public JournalEntry GetEntry(string id)
        {
            var entryCol = _db.GetCollection<JournalEntry>("journalEntry");
            entryCol.EnsureIndex(x => x.Id);

            var entryId = new ObjectId(id);

            var result = entryCol.Include(x => x.Tags)
                .FindById(entryId);
            // var test = entryCol.FindAll().First();
            // result = entryCol.FindById(test.Id);
            return result;
        }

        public JournalEntry AddEntry(JournalEntry entry)
        {
            var entryCol = _db.GetCollection<JournalEntry>("journalEntry");
            entryCol.EnsureIndex(x => x.CreatedDate);

            var id = entryCol.Insert(entry);
            return entryCol.Include(x => x.Tags).FindById(entry.Id);
        }

        public JournalEntry UpdateEntry(PartialJournalEntry partialEntry)
        {
            var entryCol = _db.GetCollection<JournalEntry>("journalEntry");

            var originalEntry = entryCol.Include(x => x.Tags).FindById(partialEntry.Id);

            if (originalEntry is null)
                return null;
            else
                originalEntry.Merge(partialEntry);

            entryCol.Update(originalEntry);
            return originalEntry;
        }

        public IEnumerable<JournalTag> GetTags()
        {
            var tagCol = _db.GetCollection<JournalTag>("journalTag");
            tagCol.EnsureIndex(x=> x.Name.ToLower());

            var result = tagCol.FindAll();
            return result;
        }

        public JournalTag AddTag(JournalTag tag)
        {
            tag.Name = tag.Name.ToLower();

            var tagCol = _db.GetCollection<JournalTag>("journalTag");
            tagCol.EnsureIndex(x=> x.Name.ToLower());

            if (!tagCol.Exists(x => x.Name == tag.Name))
                tagCol.Insert(tag);

            return tagCol.FindOne(x => x.Name == tag.Name);
        }

        public JournalTag UpdateTag(JournalTag tag)
        {
            tag.Name = tag.Name.ToLower();

            var tagCol = _db.GetCollection<JournalTag>("journalTag");
            tagCol.EnsureIndex(x=> x.Name.ToLower());

            if (tagCol.Update(tag))
                return tagCol.FindById(tag.Id);
            else
                return null;

        }
    }

    public interface IDataService
    {
        JournalEntry AddEntry(JournalEntry entry);
        JournalEntry UpdateEntry(PartialJournalEntry entry);
        JournalEntry GetEntry(string id);
        IEnumerable<JournalEntry> GetEntries();
        JournalTag AddTag(JournalTag tag);
        JournalTag UpdateTag(JournalTag tag);
        IEnumerable<JournalTag> GetTags();
    }
}