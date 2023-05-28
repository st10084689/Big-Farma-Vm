using Big_Farma.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Big_Farma.Areas.admin.Controllers
{
    [Area("admin")]
    public class farmerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public farmerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var ListFarmers = _unitOfWork.product.GetAll();
            return View(ListFarmers);
        }




    }
}
