using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreMart.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        /* var claimsIdentity = (ClaimsIdentity)User.Identity;
           var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        */

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // عرض صفحة Checkout
        public async Task<IActionResult> Checkout()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // الحصول على سلة المنتجات الخاصة بالمستخدم
            var cartItems = _unitOfWork.ShoppingCart.GetAll(
                u => u.CustomerId == claim.Value,
                Includeword: "Product"
            );

            if (!cartItems.Any())  // لو السلة فارغة
            {
                TempData["Error"] = "سلتك فارغة، يرجى إضافة منتجات أولاً.";
                return RedirectToAction("GetAll", "ShoppingCart");
            }

            // إنشاء OrderHeader
            var orderHeader = new OrderHeader
            {
                CustomerId = claim.Value,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(c => c.PriceAtThatTime * c.Count),
                Status = "Pending"
            };

            await _unitOfWork.OrderHeader.AddAsync(orderHeader);
            await _unitOfWork.CompleteAsync();

            // إضافة OrderDetails
            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    OrderId = orderHeader.Id,
                    ProductId = cartItem.ProductId,
                    Count = cartItem.Count,
                    Price = cartItem.PriceAtThatTime
                };
                await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            }
            await _unitOfWork.CompleteAsync();

            // حذف العناصر من السلة
            foreach (var cartItem in cartItems)
            {
                _unitOfWork.ShoppingCart.Remove(cartItem);
            }
             await _unitOfWork.CompleteAsync();

            // التوجيه إلى صفحة الدفع أو تأكيد الطلب
            return RedirectToAction("Payment", new { orderId = orderHeader.Id });
        }




        // GET: Customer/Order/Payment
        public async Task<IActionResult> Payment(int orderId)
        {
            var order = await _unitOfWork.OrderHeader.GetOrderHeaderWithDetails(orderId);
            return View(order);
        }

        // POST: Customer/Order/Payment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(int orderId, string stripeToken)
        {
            // TODO: integrate payment gateway here, e.g., Stripe

            var order = await _unitOfWork.OrderHeader.GetByIdAsync(orderId);
            order.Status = "Completed";
            _unitOfWork.OrderHeader.Update(order);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Confirmation", new { orderId });
        }

        // GET: Customer/Order/Confirmation
        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _unitOfWork.OrderHeader.GetOrderHeaderWithDetails(orderId);
            return View(order);
        }
    }
}
