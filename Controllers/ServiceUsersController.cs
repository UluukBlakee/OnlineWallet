using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWallet.Models;
using ServiceProvider = OnlineWallet.Models.ServiceProvider;

namespace OnlineWallet.Controllers
{
    [Authorize]
    public class ServiceUsersController : Controller
    {
        private readonly WalletContext _context;
        public ServiceUsersController(WalletContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user != null)
            {
                ServiceUser serviceUser = new ServiceUser()
                {
                    Balance = 100,
                    PhoneNumber = user.PhoneNumber,
                    AccountNumber = user.AccountNumber,
                    ServiceProviderId = id
                };
                await _context.ServiceUsers.AddAsync(serviceUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ServiceProviders", new { id = id });
            }
            return NotFound();
        }
    }
}
