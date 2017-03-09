using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NoDbProjectJournal.Repository;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NoDbProjectJournal.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IOptions<ProjectJournalOptions> projectJournalOptionsAccessor)
        {
            var options = projectJournalOptionsAccessor.Value;

            TempData["AppTitle"] = options.AppTitle;
        }
    }
}
