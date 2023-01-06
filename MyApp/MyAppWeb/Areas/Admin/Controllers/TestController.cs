using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using MyApp.Models;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(new Test());
        }
        [HttpPost]
        public IActionResult Create(Test testModel)
        {
            if (!ModelState.IsValid)
                return View("index",testModel);
            _unitOfWork.Test.Add(testModel);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
