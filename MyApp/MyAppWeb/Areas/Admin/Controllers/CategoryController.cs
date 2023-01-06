using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.CommonHelper;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebsiteRole.Role_Admin)]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            CategoryViewModel categoryVM = new CategoryViewModel();
            categoryVM.categories = _unitOfWork.Category.GetAll();
            return View(categoryVM);
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (!ModelState.IsValid)
        //        return View(category);
        //    _unitOfWork.Category.Add(category);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Record Save Successfully.";
        //    return RedirectToAction("Index");
        //}

        public IActionResult CreateUpdate(int? id)
        {
            CategoryViewModel categoryVM = new CategoryViewModel();
            if (id == null || id == 0)
                return View(categoryVM);
            categoryVM.category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (categoryVM.category == null)
                return NotFound();
            else
                return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
                return View(categoryVM);
            if (categoryVM.category.Id == 0)
            {
                _unitOfWork.Category.Add(categoryVM.category);
                _unitOfWork.Save();
                TempData["success"] = "Record Save Successfully.";
            }
            else
            {
                _unitOfWork.Category.Update(categoryVM.category);
                _unitOfWork.Save();
                TempData["success"] = "Record Update Successfully.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            CategoryViewModel categoryVM = new CategoryViewModel();
            if (id == null || id == 0)
                return NotFound();
            categoryVM.category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (categoryVM.category == null)
                return NotFound();
            _unitOfWork.Category.Delete(categoryVM.category);
            _unitOfWork.Save();
            TempData["error"] = "Record Delete Successfully.";
            return RedirectToAction("Index");
        }
        public IActionResult Test()
        {
            return View();
        }
    }
}