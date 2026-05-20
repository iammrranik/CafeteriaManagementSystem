using App.AuthFilters;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [CustomerAccess]
    public class CustomerController : Controller
    {
        private readonly MenuItemService menuService;
        private readonly MealBookingService bookingService;
        private readonly WalletTransactionService walletService;
        private readonly UserService userService;

        public CustomerController(
            MenuItemService menuService,
            MealBookingService bookingService,
            WalletTransactionService walletService,
            UserService userService)
        {
            this.menuService = menuService;
            this.bookingService = bookingService;
            this.walletService = walletService;
            this.userService = userService;
        }

        // Show recommended items for the logged-in customer
        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var user = userService.Get(userId);
            ViewBag.WalletBalance = user?.WalletBalance ?? 0.0;
            return View(menuService.GetRecommendedMenu(userId));
        }

        // Process meal booking with balance validation
        [HttpPost]
        public IActionResult BookMeal(MealBookingDTO obj)
        {
            obj.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (bookingService.Create(obj))
            {
                TempData["Msg"] = "Booking confirmed.";
                return RedirectToAction("MyBookings");
            }

            TempData["Msg"] = "Booking failed. Check your wallet balance or daily booking lock (before 9.00 AM for today).";
            return RedirectToAction("Index");
        }

        // Display personal order history
        public IActionResult MyBookings()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var bookings = bookingService.Get().Where(b => b.UserId == userId).ToList();
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Recharge()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var user = userService.Get(userId);
            ViewBag.CurrentBalance = user?.WalletBalance ?? 0.0;
            return View(new WalletTransactionDTO { UserId = userId, TransactionType = "Deposit" });
        }

        [HttpPost]
        public IActionResult Recharge(WalletTransactionDTO obj)
        {
            obj.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            obj.TransactionType = "Deposit";
            if (ModelState.IsValid)
            {
                if (walletService.Create(obj))
                {
                    TempData["Msg"] = "Wallet recharged successfully!";
                    return RedirectToAction("Index");
                }
                TempData["Msg"] = "Failed to recharge. Amount must be positive.";
            }
            var user = userService.Get(obj.UserId);
            ViewBag.CurrentBalance = user?.WalletBalance ?? 0.0;
            return View(obj);
        }
    }
}
