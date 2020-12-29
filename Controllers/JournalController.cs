using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LiteDB;
using kwisatz.Data;
using kwisatz.Data.Entities;
using kwisatz.Models;

namespace kwisatz.Controllers
{
    [ApiController]
    [Route("api/journal")]
    public class JournalController : ControllerBase
    {
        private readonly ILogger<JournalController> _logger;
        private readonly IDataService _dataService;
        public JournalController(ILogger<JournalController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        [HttpGet("entries")]
        public ActionResult<IEnumerable<JournalEntryModel>> GetEntries()
        {
            var entries = _dataService.GetEntries();

            return entries.Select(x => new JournalEntryModel(x)).ToList();
        }

        [HttpGet("entries/{id}")]
        public ActionResult<JournalEntryModel> GetEntry(string id)
        {
            var entry = _dataService.GetEntry(id);

            return new JournalEntryModel(entry);
        }

        [HttpPost("entries")]
        public ActionResult<JournalEntryModel> PostEntry(JournalEntrySubmission submission)
        {
            var tags = SaveTags(submission.Tags);

            var newEntry = new JournalEntry(submission.Title, submission.Body, tags);
            newEntry = _dataService.AddEntry(newEntry);

            var responseModel = new JournalEntryModel(newEntry);

            return CreatedAtAction(nameof(GetEntry), new { id = responseModel.Id }, responseModel);
        }

        [HttpPatch("entries/{id}")]
        public ActionResult<JournalEntryModel> UpdateEntry(string id, JournalEntryUpdate update)
        {
            var tags = update.Tags != null ? SaveTags(update.Tags) : null;

            var partialEntry = new PartialJournalEntry(id, update.Title, update.Body, tags);
            var updatedEntry = _dataService.UpdateEntry(partialEntry);

            if (updatedEntry is null)
                return NotFound();

            var responseModel = new JournalEntryModel(updatedEntry);

            return Ok(responseModel);
        }

        [HttpGet("tags")]
        public ActionResult<IEnumerable<JournalTagModel>> GetTags()
        {
            var tags = _dataService.GetTags();

            return tags.Select(x => new JournalTagModel(x)).ToList();
        }

        [HttpPost("tags")]
        public ActionResult<JournalTagModel> PostTag(JournalTagSubmission submission)
        {
            var newTag = _dataService.AddTag(new JournalTag { Name = submission.Name });

            var responseModel = new JournalTagModel(newTag);

            return CreatedAtAction(nameof(GetTags), new { id = responseModel.Id }, responseModel);
        }

        [HttpPut("tags/{id}")]
        public ActionResult<JournalTagModel> PutTag(int id, JournalTagSubmission submission)
        {
            var updateTag = _dataService.UpdateTag(new JournalTag { Id = id, Name = submission.Name });

            if (updateTag is null)
                return NotFound();

            var responseModel = new JournalTagModel(updateTag);

            return Ok(responseModel);
        }

        private List<JournalTag> SaveTags(IEnumerable<string> incomingTags)
        {
            var tags = new List<JournalTag>();

            foreach (var tagName in incomingTags)
            {
                var tag = _dataService.AddTag(new JournalTag { Name = tagName });
                tags.Add(tag);
            }

            return tags;
        }
    }
}
