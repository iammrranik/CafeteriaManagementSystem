using App.AuthFilters;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace App.Controllers
{
    [AdminAccess]
    public class AdminController : Controller
    {
        private readonly UserService userService;
        private readonly MealBookingService bookingService;
        private readonly MenuItemService menuItemService;
        private readonly SystemLogService logService;
        private readonly WalletTransactionService walletService;
        private readonly UserTypeService userTypeService;

        public AdminController(
            UserService userService,
            MealBookingService bookingService,
            MenuItemService menuItemService,
            SystemLogService logService,
            WalletTransactionService walletService,
            UserTypeService userTypeService)
        {
            this.userService = userService;
            this.bookingService = bookingService;
            this.menuItemService = menuItemService;
            this.logService = logService;
            this.walletService = walletService;
            this.userTypeService = userTypeService;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // Users CRUD
        public IActionResult Users()
        {
            return View(userService.Get());
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.Types = userTypeService.Get();
            return View(new UserDTO());
        }

        [HttpPost]
        public IActionResult CreateUser(UserDTO obj)
        {
            if (ModelState.IsValid)
            {
                if (userService.Create(obj))
                {
                    TempData["Msg"] = "User Created Successfully";
                    return RedirectToAction("Users");
                }
            }
            ViewBag.Types = userTypeService.Get();
            return View(obj);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var obj = userService.Get(id);
            if (obj == null) return NotFound();
            ViewBag.Types = userTypeService.Get();
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditUser(UserDTO obj)
        {
            int adminId = GetAdminId();
            
            // Bypass validation for unchanged password
            var originalUser = userService.Get(obj.Id);
            if (originalUser != null && obj.Password == originalUser.Password)
            {
                ModelState.Remove("Password");
            }

            if (ModelState.IsValid)
            {
                var exists = originalUser != null;
                var updated = userService.Update(obj, adminId);
                if (updated || exists)
                {
                    TempData["Msg"] = "User Updated Successfully";
                    return RedirectToAction("Users");
                }
            }
            ViewBag.Types = userTypeService.Get();
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            int adminId = GetAdminId();
            try
            {
                if (userService.Delete(id, adminId))
                {
                    TempData["Msg"] = "User Deleted Successfully";
                }
                else
                {
                    TempData["Msg"] = "Failed to Delete User";
                }
            }
            catch (Exception)
            {
                TempData["Msg"] = "Cannot delete user. They have active bookings or system logs associated with them.";
            }
            return RedirectToAction("Users");
        }

        // Bookings CRUD & Bulk Actions
        public IActionResult Bookings()
        {
            return View(bookingService.Get());
        }

        [HttpPost]
        public IActionResult CancelDay(DateTime bookingDate)
        {
            if (bookingService.CancelAndRefundEntireDay(bookingDate))
                TempData["Msg"] = "Cancellation and refund successful.";
            else
                TempData["Msg"] = "No bookings found for the selected date.";

            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public IActionResult EmergencyShutdown(DateTime date)
        {
            var success = bookingService.CancelAndRefundEntireDay(date);
            TempData["Msg"] = success ? "Day processed successfully." : "No bookings found.";
            return RedirectToAction("Bookings");
        }

        [HttpGet]
        public IActionResult CreateBooking()
        {
            ViewBag.Users = userService.Get();
            ViewBag.MenuItems = menuItemService.Get();
            return View(new MealBookingDTO { BookingDate = DateTime.Today });
        }

        [HttpPost]
        public IActionResult CreateBooking(MealBookingDTO obj)
        {
            ModelState.Remove("Status");
            ModelState.Remove("TotalPrice");
            if (ModelState.IsValid)
            {
                if (bookingService.Create(obj))
                {
                    TempData["Msg"] = "Booking Created Successfully";
                    return RedirectToAction("Bookings");
                }
                TempData["Msg"] = "Booking creation failed. Check balance/stock or timing (before 9 AM for today).";
            }
            ViewBag.Users = userService.Get();
            ViewBag.MenuItems = menuItemService.Get();
            return View(obj);
        }

        [HttpGet]
        public IActionResult EditBooking(int id)
        {
            var obj = bookingService.Get(id);
            if (obj == null) return NotFound();
            ViewBag.Users = userService.Get();
            ViewBag.MenuItems = menuItemService.Get();
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditBooking(MealBookingDTO obj)
        {
            if (ModelState.IsValid)
            {
                var exists = bookingService.Get(obj.Id) != null;
                var updated = bookingService.Update(obj);
                if (updated || exists)
                {
                    TempData["Msg"] = "Booking Processed Successfully";
                    return RedirectToAction("Bookings");
                }
            }
            ViewBag.Users = userService.Get();
            ViewBag.MenuItems = menuItemService.Get();
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                if (bookingService.Delete(id))
                {
                    TempData["Msg"] = "Booking Deleted Successfully";
                }
                else
                {
                    TempData["Msg"] = "Failed to Delete Booking";
                }
            }
            catch (Exception)
            {
                TempData["Msg"] = "Cannot delete booking due to a system constraint.";
            }
            return RedirectToAction("Bookings");
        }

        // Menu Items CRUD
        public IActionResult MenuItems()
        {
            return View(menuItemService.Get());
        }

        [HttpGet]
        public IActionResult CreateMenuItem()
        {
            return View(new MenuItemDTO());
        }

        [HttpPost]
        public IActionResult CreateMenuItem(MenuItemDTO obj)
        {
            if (ModelState.IsValid)
            {
                if (menuItemService.Create(obj))
                {
                    TempData["Msg"] = "Menu Item Created Successfully";
                    return RedirectToAction("MenuItems");
                }
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult EditMenuItem(int id)
        {
            var obj = menuItemService.Get(id);
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditMenuItem(MenuItemDTO obj)
        {
            if (ModelState.IsValid)
            {
                var exists = menuItemService.Get(obj.Id) != null;
                var updated = menuItemService.Update(obj);
                if (updated || exists)
                {
                    TempData["Msg"] = "Menu Item Updated Successfully";
                    return RedirectToAction("MenuItems");
                }
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeleteMenuItem(int id)
        {
            try
            {
                if (menuItemService.Delete(id))
                {
                    TempData["Msg"] = "Menu Item Deleted Successfully";
                }
                else
                {
                    TempData["Msg"] = "Failed to Delete Menu Item";
                }
            }
            catch (Exception)
            {
                TempData["Msg"] = "Cannot delete menu item. It is referenced by existing meal bookings.";
            }
            return RedirectToAction("MenuItems");
        }

        // UserType CRUD
        public IActionResult UserTypes()
        {
            return View(userTypeService.Get());
        }

        [HttpGet]
        public IActionResult CreateUserType()
        {
            return View(new UserTypeDTO());
        }

        [HttpPost]
        public IActionResult CreateUserType(UserTypeDTO obj)
        {
            if (ModelState.IsValid)
            {
                if (userTypeService.Create(obj))
                {
                    TempData["Msg"] = "User Type Created Successfully";
                    return RedirectToAction("UserTypes");
                }
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult EditUserType(int id)
        {
            var obj = userTypeService.Get(id);
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditUserType(UserTypeDTO obj)
        {
            if (ModelState.IsValid)
            {
                var exists = userTypeService.Get(obj.Id) != null;
                var updated = userTypeService.Update(obj);
                if (updated || exists)
                {
                    TempData["Msg"] = "User Type Updated Successfully";
                    return RedirectToAction("UserTypes");
                }
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeleteUserType(int id)
        {
            try
            {
                if (userTypeService.Delete(id))
                {
                    TempData["Msg"] = "User Type Deleted Successfully";
                }
                else
                {
                    TempData["Msg"] = "Failed to Delete User Type";
                }
            }
            catch (Exception)
            {
                TempData["Msg"] = "Cannot delete user type. It is currently assigned to registered users.";
            }
            return RedirectToAction("UserTypes");
        }

        // Financial oversight CRUD (Auditing transactions)
        public IActionResult FinancialAudit()
        {
            return View(walletService.Get());
        }

        [HttpGet]
        public IActionResult RecordTransaction()
        {
            ViewBag.Users = userService.Get();
            return View(new WalletTransactionDTO());
        }

        [HttpPost]
        public IActionResult RecordTransaction(WalletTransactionDTO obj)
        {
            if (ModelState.IsValid)
            {
                if (walletService.Create(obj))
                {
                    TempData["Msg"] = "Transaction Recorded Successfully";
                    return RedirectToAction("FinancialAudit");
                }
                TempData["Msg"] = "Transaction failed (amount must be positive).";
            }
            ViewBag.Users = userService.Get();
            return View(obj);
        }

        [HttpPost]
        public IActionResult DeleteTransaction(int id)
        {
            try
            {
                if (walletService.Delete(id))
                {
                    TempData["Msg"] = "Transaction Deleted Successfully";
                }
                else
                {
                    TempData["Msg"] = "Failed to Delete Transaction";
                }
            }
            catch (Exception)
            {
                TempData["Msg"] = "Cannot delete transaction due to auditing integrity locks.";
            }
            return RedirectToAction("FinancialAudit");
        }

        // Logs and Security auditing
        public IActionResult Logs()
        {
            return View(logService.Get());
        }

        public IActionResult SecurityLogs()
        {
            return View(logService.Get());
        }

        [HttpPost]
        public IActionResult DeleteLog(int id)
        {
            if (logService.Delete(id))
            {
                TempData["Msg"] = "Log Deleted Successfully";
            }
            else
            {
                TempData["Msg"] = "Failed to Delete Log";
            }
            return RedirectToAction("Logs");
        }

        // Intelligent Trending/Recommendations Display
        public IActionResult TrendingItems(int userId)
        {
            return View(menuItemService.GetRecommendedMenu(userId));
        }

        private int GetAdminId()
        {
            int adminId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (adminId == 0)
            {
                var uname = HttpContext.Session.GetString("Uname");
                if (!string.IsNullOrEmpty(uname))
                {
                    var user = userService.Get().FirstOrDefault(u => u.IdCardNo == uname);
                    if (user != null)
                    {
                        adminId = user.Id;
                    }
                }
            }
            if (adminId == 0)
            {
                var fallback = userService.Get().FirstOrDefault(u => u.UserTypeId == 1);
                if (fallback != null)
                {
                    adminId = fallback.Id;
                }
            }
            return adminId;
        }
    }
}
