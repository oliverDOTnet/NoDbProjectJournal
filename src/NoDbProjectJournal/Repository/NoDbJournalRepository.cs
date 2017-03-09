using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoDbProjectJournal.Models;

namespace NoDbProjectJournal.Repository
{
    public class NoDbJournalRepository : IJournalRepository
    {
        private IBasicCommands<JournalEntry> commands;

        private IBasicQueries<JournalEntry> query;

        private ILogger log;

        private readonly string PROJECT_ID = "default";

        public NoDbJournalRepository(
            IBasicCommands<JournalEntry> journalCommands,
            IBasicQueries<JournalEntry> journalQueries,
            ILogger<NoDbJournalRepository> logger)
        {
            commands = journalCommands;
            query = journalQueries;
            log = logger;
        }

        public async Task<IEnumerable<JournalEntry>> GetAllJournals()
        {
            return await query.GetAllAsync(PROJECT_ID);
        }

        public Task<IEnumerable<JournalEntry>> GetAllJournalsByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<JournalEntry> GetJournalById(string journalId)
        {
            return await query.FetchAsync(PROJECT_ID, journalId);
        }

        public async Task Create(JournalEntry entry)
        {
            string journalId = entry.Timestamp.Ticks.ToString();
            entry.Id = journalId;

            await commands.CreateAsync(PROJECT_ID, entry.Id, entry);
        }

        public async Task Update(JournalEntry entry)
        {
            await commands.UpdateAsync(PROJECT_ID, entry.Id, entry);
        }

        public async Task DeleteJournalById(string journalId)
        {
            await commands.DeleteAsync(PROJECT_ID, journalId);
        }
    }
}
