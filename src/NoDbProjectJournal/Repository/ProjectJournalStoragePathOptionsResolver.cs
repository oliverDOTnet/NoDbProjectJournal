using Microsoft.Extensions.Options;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoDbProjectJournal.Repository
{
    public class ProjectJournalStoragePathOptionsResolver : IStoragePathOptionsResolver
    {
        private readonly ProjectJournalOptions options;

        public ProjectJournalStoragePathOptionsResolver(IOptions<ProjectJournalOptions> projectJournalOptionsAccessor)
        {
            options = projectJournalOptionsAccessor.Value;
        }

        public Task<StoragePathOptions> Resolve(string projectId)
        {
            var result = new StoragePathOptions();

            result.AppRootFolderPath = options.NoDbDefaultPath;

            return Task.FromResult(result);
        }
    }
}
