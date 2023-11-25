using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineWallet.Models;
using ServiceProvider = OnlineWallet.Models.ServiceProvider;

namespace OnlineWallet.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly WalletContext _context;
        public TransactionsController(WalletContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            List<Transaction> transactions = await _context.Transactions.Include(t => t.SenderUser).Include(t => t.ReceiverUser).Include(s => s.Services).Where(t => t.SenderUserId == user.Id || t.ReceiverUserId == user.Id).ToListAsync();

            if (fromDate != null)
            {
                transactions = transactions.Where(t => t.Date >= fromDate).ToList();
            }
            if (toDate != null)
            {
                transactions = transactions.Where(t => t.Date <= toDate).ToList();
            }
            return View(transactions);
        }

        public async Task<IActionResult> GetBalance()
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> Replenish(string accountNumber, int amountMoney)
        {
            if (amountMoney <= 0)
            {
                return Json("Сумма должна быть положительной");
            }

            User user = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == accountNumber);
            if (user != null)
            {
                user.Balance += amountMoney;
                _context.Users.Update(user);

                Transaction transaction = new Transaction()
                {
                    ReceiverUserId = user.Id,
                    Amount = amountMoney,
                    Date = DateTime.UtcNow,
                    Type = "Пополнение"
                };
                await _context.Transactions.AddAsync(transaction);
                int result = await _context.SaveChangesAsync();
                if (result > 0)
                    return Json("Операция прошла успешно");
                else
                    return Json("Произошла ошибка при выполнении операции");
            }
            else
                return Json("Пользователь не найден");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Transfer(string accountNumber, int amountMoney)
        {
            User fromUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            User toUser = await _context.Users.FirstOrDefaultAsync(u => u.AccountNumber == accountNumber);

            if (fromUser != null && toUser != null && fromUser != toUser)
            {
                if (amountMoney > 0 && fromUser.Balance >= amountMoney)
                {
                    fromUser.Balance -= amountMoney;
                    _context.Users.Update(fromUser);

                    toUser.Balance += amountMoney;
                    _context.Users.Update(toUser);

                    Transaction transaction = new Transaction()
                    {
                        SenderUserId = fromUser.Id,
                        ReceiverUserId = toUser.Id,
                        Amount = amountMoney,
                        Date = DateTime.UtcNow,
                        Type = "Перевод"
                    };
                    await _context.Transactions.AddAsync(transaction);
                    await _context.SaveChangesAsync();

                    return Json("Операция прошла успешно");
                }
                else
                    return Json("Недостаточно средств или некорректная сумма для перевода");
            }
            else
                return Json("Некорректные данные отправителя или получателя");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Payment(string service, string accountNumber, int amountMoney)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            ServiceProvider serviceProvider = await _context.ServiceProviders.FirstOrDefaultAsync(s => s.Name == service);

            if (user != null && serviceProvider != null)
            {
                if (amountMoney > 0 && user.Balance >= amountMoney)
                {
                    ServiceUser serviceUser = await _context.ServiceUsers.FirstOrDefaultAsync(s => s.ServiceProviderId == serviceProvider.Id && s.AccountNumber == accountNumber || s.PhoneNumber == accountNumber);

                    if (serviceUser == null)
                    {
                        return Json("Вы не пользуетесь услугами этой компании");
                    }

                    user.Balance -= amountMoney;
                    _context.Users.Update(user);

                    serviceUser.Balance += amountMoney;
                    _context.ServiceUsers.Update(serviceUser);

                    Transaction transaction = new Transaction()
                    {
                        SenderUserId = user.Id,
                        ServicesId = serviceProvider.Id,
                        Amount = amountMoney,
                        Date = DateTime.UtcNow,
                        Type = "Оплата услуг"
                    };
                    await _context.Transactions.AddAsync(transaction);
                    await _context.SaveChangesAsync();

                    return Json("Операция прошла успешно");
                }
                else
                    return Json("Недостаточно средств или некорректная сумма для перевода");
            }
            else
                return Json("Некорректные данные отправителя или получателя");
        }
    }
}
