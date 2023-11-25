using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWallet.Models;
using System.Diagnostics;
using ServiceProvider = OnlineWallet.Models.ServiceProvider;

namespace OnlineWallet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WalletContext _context;
        public HomeController(ILogger<HomeController> logger, WalletContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ServiceProvider> serviceProviders = await _context.ServiceProviders.ToListAsync();
                ViewBag.Services = serviceProviders;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                return View(user);
            }
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}