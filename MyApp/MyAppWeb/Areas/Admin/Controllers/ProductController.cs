using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.CommonHelper;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;
using System.Data;
using System.IO;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebsiteRole.Role_Admin)]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnvirnment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvirnment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvirnment = hostingEnvirnment;
        }

        public IActionResult Index()
        {
            ProductViewModel productVM = new ProductViewModel();
            productVM.Products = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(productVM);
        }

        public IActionResult GetAllProduct()
        {
            var Products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = Products });
        }

        public IActionResult CreateUpdate(int? id)
        {
            ProductViewModel productVM = new ProductViewModel()
            {
                Product = new(),
                Categories = _unitOfWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id == null || id == 0)
                return View(productVM);
            productVM.Product = _unitOfWork.Product.GetT(x => x.Id == id);
            if (productVM.Product == null)
                return NotFound();
            else
                return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductViewModel productVM, IFormFile? file)
        {
            string fileName = string.Empty;
            if (!ModelState.IsValid)
                return View(productVM);
            if (file != null)
            {
                string uploadDir = Path.Combine(_hostingEnvirnment.WebRootPath, "ProductImage");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                if (productVM.Product.ImageUrl!=null)
                {
                    var oldImagePath = Path.Combine(_hostingEnvirnment.WebRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);    
                }
                productVM.Product.ImageUrl = @"\ProductImage\"+fileName;
            }
            if (productVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Record Save Successfully.";
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Record Update Successfully.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            ProductViewModel productVM = new ProductViewModel();
            if (id == null || id == 0)
                return NotFound();
            productVM.Product = _unitOfWork.Product.GetT(x => x.Id == id);
            if (productVM.Product == null)
                return NotFound();
            var oldImagePath = Path.Combine(_hostingEnvirnment.WebRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);
            _unitOfWork.Product.Delete(productVM.Product);
            _unitOfWork.Save();
            TempData["error"] = "Record Delete Successfully.";
            return RedirectToAction("Index");
        }
    }
}