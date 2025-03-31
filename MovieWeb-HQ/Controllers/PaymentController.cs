using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieWeb_HQ.Interface;
using MovieWeb_HQ.Models;
using MovieWeb_HQ.Models.Order;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MovieWeb_HQ.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public PaymentController(IMomoService momoService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _momoService = momoService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack()
        {
            // Xử lý phản hồi từ MoMo
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);

            // Lấy UserId từ Identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    // Xóa role "User" (nếu có) và thêm "Member"
                    await _userManager.RemoveFromRoleAsync(user, "User");
                    await _userManager.AddToRoleAsync(user, "Member");
                }
            }

            // Chuyển hướng đến trang hiển thị kết quả
            TempData["PaymentMessage"] = "✅ Thanh toán thành công! Bạn đã được nâng cấp lên thành viên.";
            return View(response);
        }


    }
}
