using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWallet.Models;
using ServiceProvider = OnlineWallet.Models.ServiceProvider;

namespace OnlineWallet.Controllers
{
    [Authorize]
    public class ServiceProvidersController : Controller
    {
        private readonly WalletContext _context;
        public ServiceProvidersController(WalletContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<ServiceProvider> serviceProviders = await _context.ServiceProviders.ToListAsync();
            return View(serviceProviders);
        }
        public async Task<IActionResult> Details(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            ServiceProvider serviceProvider = await _context.ServiceProviders.FirstOrDefaultAsync(s => s.Id == id);
            if (user != null)
            {
                ServiceUser serviceUser = await _context.ServiceUsers.FirstOrDefaultAsync(s => s.ServiceProviderId == id && s.AccountNumber == user.AccountNumber);
                if (serviceUser == null)
                {
                    ViewBag.UsesService = false;
                }
                else
                {
                    ViewBag.UsesService = true;
                    ViewBag.ServiceUser = serviceUser;
                }
                return View(serviceProvider);
            }
            return NotFound();
        }
    }
}
