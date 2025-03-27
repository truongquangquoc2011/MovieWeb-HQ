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
        [HttpPost]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);

            if (response == null)
            {
                TempData["PaymentMessage"] = "Lỗi: Không nhận được phản hồi từ MoMo!";
                return RedirectToAction("PaymentFail");
            }

            Console.WriteLine($"MoMo Response: {JsonConvert.SerializeObject(response)}");

            if (response.Status == "0") // "0" = Thành công
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var isUser = await _userManager.IsInRoleAsync(user, "User");
                        if (isUser)
                        {
                            await _userManager.RemoveFromRoleAsync(user, "User");
                            await _userManager.AddToRoleAsync(user, "Member");
                        }
                    }
                }

                TempData["PaymentMessage"] = "Thanh toán thành công! Cảm ơn bạn đã đăng ký hội viên.";
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                TempData["PaymentMessage"] = "Thanh toán không thành công. Vui lòng thử lại!";
                return RedirectToAction("PaymentFail");
            }
        }
        public IActionResult PaymentSuccess()
        {
            ViewBag.Message = TempData["PaymentMessage"];
            return View();
        }

        public IActionResult PaymentFail()
        {
            ViewBag.Message = TempData["PaymentMessage"];
            return View();
        }

    }
}
