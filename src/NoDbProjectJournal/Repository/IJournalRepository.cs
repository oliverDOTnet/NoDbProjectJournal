using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoDbProjectJournal.Models;

namespace NoDbProjectJournal.Repository
{
    public interface IJournalRepository
    {
        Task Create(JournalEntry entry);

        Task Update(JournalEntry entry);

        Task DeleteJournalById(string journalId);

        Task<JournalEntry> GetJournalById(string journalId);

        Task<IEnumerable<JournalEntry>> GetAllJournals();

        Task<IEnumerable<JournalEntry>> GetAllJournalsByUserName(string username);
    }
}
