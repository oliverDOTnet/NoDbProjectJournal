using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoDbProjectJournal.Repository;
using Microsoft.Extensions.Logging;
using NoDbProjectJournal.Models;
using System.Text;

namespace NoDbProjectJournal.Controllers
{
    public class HomeController : Controller
    {
        private IJournalRepository repository;
        private ILogger log;

        public HomeController(IJournalRepository journalRepository)
        {
            repository = journalRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<JournalEntry> result = await repository.GetAllJournals();

            if (TempData.ContainsKey("EditMode"))
            {
                if (!(bool)TempData["EditMode"])
                {
                    ViewData["SingleModel"] = new JournalEntry();
                    ViewData["EditFormTitle"] = "New Entry";
                    ViewData["EditFormAction"] = "Create";
                }
                else
                {
                    string editJournalId = TempData["EditJournalId"].ToString();
                    JournalEntry editJournal = await repository.GetJournalById(editJournalId);

                    if (editJournal != null)
                    {
                        ViewData["SingleModel"] = editJournal;
                        ViewData["EditFormTitle"] = "Edit Entry";
                        ViewData["EditFormAction"] = "Edit";
                    }
                    else
                    {
                        // TODO: Error Message
                    }
                }
            }
            else
            {
                TempData["EditMode"] = false;

                ViewData["SingleModel"] = new JournalEntry();
                ViewData["EditFormTitle"] = "New Entry";
                ViewData["EditFormAction"] = "Create";
            }

            return View(result.OrderByDescending(j => j.Timestamp.Ticks));
        }

        [HttpGet]
        public async Task<IActionResult> MyJournal()
        {
            IEnumerable<JournalEntry> result = await repository.GetAllJournals();

            TempData["EditMode"] = false;

            ViewData["SingleModel"] = new JournalEntry();
            ViewData["EditFormTitle"] = "New Entry";
            ViewData["EditFormAction"] = "Create";

            return View("Index", result.Where(j => j.UserName.Equals(HttpContext.User.Identity.Name.ToUpper())).OrderByDescending(j => j.Timestamp.Ticks));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string content)
        {
            DateTime now = DateTime.Now;
            DateTime timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            JournalEntry newEntry = new JournalEntry()
            {
                Content = content,
                Timestamp = timestamp,
                UserName = HttpContext.User.Identity.Name.ToUpper()
            };

            if (TryValidateModel(newEntry))
            {
                await repository.Create(newEntry);
            }
            else
            {
                // TODO: Error Message
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //JournalEntry editEntry = await repository.GetJournalById(id);

            TempData["EditMode"] = true;
            TempData["EditJournalId"] = id;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JournalEntry editEntry)
        {
            TempData["EditMode"] = false;

            await repository.Update(editEntry);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await repository.DeleteJournalById(id);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
