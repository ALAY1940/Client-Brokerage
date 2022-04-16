using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MarketDbContext _context;

        public ClientsController(MarketDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var clientViewModel = new BrokerageViewModel();
            clientViewModel.Clients = await _context.Clients
                .Include(client => client.Subscriptions)
                .ThenInclude(client => client.Brokerage)
                .AsNoTracking()
                .ToListAsync();

            if (id != null)
            {
                ViewData["ClientID"] = id;
                clientViewModel.Subscriptions = clientViewModel.Clients.Where(client => client.ID == id).Single().Subscriptions;
            }

            return View(clientViewModel);
        }

        public async Task<IActionResult> EditSubscriptions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            BrokerageViewModel clientViewModel = new BrokerageViewModel();
            clientViewModel.Subscriptions = await _context.Subscriptions.Where(brokerageMember => brokerageMember.ClientId == id).ToListAsync();
            clientViewModel.Clients = await _context.Clients.Where(client => client.ID == id).ToListAsync();
            clientViewModel.Brokerages = await _context.Brokerages.ToListAsync();
            return View(clientViewModel);
        }

        public async Task<IActionResult> AddSubscriptions(int? id, string brokerageId)
        {
            if (id == null || String.IsNullOrEmpty(brokerageId))
            {
                return NotFound();
            }
            var s = await _context.Clients.FindAsync(id);
            var c = await _context.Brokerages.FindAsync(brokerageId);
            if (s == null || c == null)
            {
                return NotFound();
            }
            var createBrokerageSubscription = new Subscription();
            createBrokerageSubscription.ClientId = (int)id;
            createBrokerageSubscription.BrokerageId = brokerageId;
            _context.Subscriptions.Add(createBrokerageSubscription);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditSubscriptions", new { id = id });
        }

        public async Task<IActionResult> DeleteSubscriptions(int? id, string brokerageId)
        {
            if (id == null || String.IsNullOrEmpty(brokerageId))
            {
                return NotFound();
            }
            var brokerage = await _context.Subscriptions.FindAsync(id, brokerageId);
            if (brokerage == null)
            {
                return NotFound();
            }
            _context.Subscriptions.Remove(brokerage);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditSubscriptions", new { id = id });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client= await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,BirthDate")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,BirthDate")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }
    }
}