using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWallet.Models;

namespace OnlineWallet.Controllers
{
    public class UsersController : Controller
    {
        private readonly WalletContext _context;
        public UsersController(WalletContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Replenish(string accountNumber, int amountMoney)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u =>  u.AccountNumber == accountNumber);
            if (user != null)
            {
                if (amountMoney >= 0)
                {
                    user.Balance += amountMoney;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return Json("Операция прошла успешна");
                }
            }
            return Json("Произошло ошибка при выполнении операции");
        }
    }
}
