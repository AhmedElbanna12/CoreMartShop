using CoreMart.BLL.Repository.Implementation;
using CoreMart.BLL.Repository.Interface;
using CoreMart.DAL.Models;
using CoreMart.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreMart.PL.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles =("Admin"))]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;


        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult GetData()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = unitOfWork.OrderHeader.GetAll(include: o => o.ApplicationUser);
            return Json(new { data = orderHeaders });
        }



        public async Task<IActionResult> Details(int orderid)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = unitOfWork.OrderHeader.GetAll(u => u.Id == orderid, o => o.ApplicationUser).FirstOrDefault(),
                OrderDetails = await unitOfWork.OrderDetails.GetOrderDetailsByOrderId(orderid)
            };
            return View(orderVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails(OrderVM OrderVM)
        {
            var orderfromdb = unitOfWork.OrderHeader.GetAll()
                                 .FirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);



            orderfromdb.FullName = OrderVM.OrderHeader.FullName;
            orderfromdb.Phone = OrderVM.OrderHeader.Phone;
            orderfromdb.Address = OrderVM.OrderHeader.Address;

            if (OrderVM.OrderHeader.Carrier != null)
            {
                orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderfromdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            unitOfWork.OrderHeader.Update(orderfromdb);
            await unitOfWork.CompleteAsync();

            return RedirectToAction("Details", "Order", new { orderid = orderfromdb.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartProcess()
        {
            unitOfWork.OrderHeader.UpdateOrderStatus(OrderVM.OrderHeader.Id, "Processing", null);
            await unitOfWork.CompleteAsync();
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> StartShip()
        {
            var orderfromdb = await unitOfWork.OrderHeader.GetByIdAsync(OrderVM.OrderHeader.Id);
            orderfromdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderfromdb.Carrier = OrderVM.OrderHeader.Carrier;
            orderfromdb.OrderStatus = "Shipped";
            orderfromdb.ShippingDate = DateTime.Now;

            unitOfWork.OrderHeader.Update(orderfromdb);
            unitOfWork.CompleteAsync();

            return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
        }
    }
}
