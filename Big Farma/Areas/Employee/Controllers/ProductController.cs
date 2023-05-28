using Big_Farma.Models;
using Big_Farma.Repository.IRepository;
using Big_Farma.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Security.Claims;

namespace Big_Farma.Areas.admin.Controllers
{
    [Area("Employee")]
    [Authorize]
    public class ProductController : Controller
    {
        public CustomerStockVm CustomerStock { get; set; }

          private readonly IUnitOfWork _unitOfWork;
          private readonly IWebHostEnvironment _HostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment HostEnviroment)
    {
        _unitOfWork = unitOfWork;
        _HostEnvironment = HostEnviroment;
    }
        [Authorize]
        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);


            if (User.IsInRole("Employee"))
            {
                CustomerStock = new CustomerStockVm()
                {
                    ListProducts = _unitOfWork.product.GetAll(includeProperties: "category,applicationUser")
                };
            }
            else
            {
                CustomerStock = new CustomerStockVm()
                {
                    ListProducts = _unitOfWork.product.GetAll(u => u.ApplicationIdentity == claim.Value, includeProperties: "category,applicationUser")
                };
            }

            return View(CustomerStock.ListProducts); ;

        }

        //get method of upsert
       
        public IActionResult Upsert(int? id)
    {
        ProductVm productVm = new()
        {
            product = new(),
            CategoryLists = _unitOfWork.category.GetAll().Select(i => new SelectListItem
            {
                Text = i.CategoryName,
                Value = i.ID.ToString(),
            }),
           
        };

        

        if (id == null || id == 0)
        {
            //create product


            return View(productVm);

        }
        else
        {
            //update the product
            productVm.product = _unitOfWork.product.GetFirstOrDefault(i => i.ID == id);
            return View(productVm);
        }
    }
     


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVm obj, IFormFile? file)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            obj.product.ApplicationIdentity = claim.Value;

            if (ModelState.IsValid)
            {



                string wwwRootPath = _HostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.product.ImageUrl = @"\images\products\" + fileName + extension;

                }

                if (obj.product.ID == 0)
                {

                    _unitOfWork.product.Add(obj.product);
                }
                else
                {
                    _unitOfWork.product.Update(obj.product);
                }


                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.product == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.product
                .GetFirstOrDefault(u => u.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_unitOfWork.product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var product =  _unitOfWork.product.GetFirstOrDefault(u=>u.ID==id);
            if (product != null)
            {
                _unitOfWork.product.Remove(product);
            }

             _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.product == null)
            {
                return NotFound();
            }
            ProductVm obj = new()
            {
                product = _unitOfWork.product
                .GetFirstOrDefault(m => m.ID == id, includeProperties:"category")

            };
           
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }




       

    }


}
