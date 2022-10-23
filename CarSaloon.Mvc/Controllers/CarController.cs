using CarSaloon.Services.Services.UoW;
using CarSaloon.Services.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CarSaloon.Mvc.Controllers
{
    public class CarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CarController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories=_unitOfWork.Categories.GetAllAsync().Result.Select(e=>e.Name).ToList();
            ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
            return View(await _unitOfWork.Cars.GetIndexMethodAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchText,string manufacturer,string category,int pageIndex=1,int pageSize=4)
        {
            ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
            ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
            return View(await _unitOfWork.Cars.PostIndexMethodAsync(searchText, manufacturer,category,pageIndex,pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
            ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors=await _unitOfWork.Cars.ValidateCarAsync(model);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
                    ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
                    return View(model);
                }

                string imagesFolderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images/Cars");

                await _unitOfWork.Cars.PostCreateMethodAsync(model,imagesFolderPath);
                await _unitOfWork.Commit();
                int lastCarId = _unitOfWork.Cars.GetAllAsync().Result.LastOrDefault().Id;
                return RedirectToAction(nameof(Edit), new { id = lastCarId });
            }
            else
            {
                ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
                ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
            ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
            return View(await _unitOfWork.Cars.GetEditMethodAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors=await _unitOfWork.Cars.ValidateCarAsync(model);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
                    ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
                    return View(model);
                }

                string imagesFolderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images/Cars");

                await _unitOfWork.Cars.PostEditMethodAsync(model,imagesFolderPath);
                await _unitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Categories = _unitOfWork.Categories.GetAllAsync().Result.Select(e => e.Name).ToList();
                ViewBag.Manufacturers = _unitOfWork.Manufacturers.GetAllAsync().Result.Select(e => e.Name).ToList();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _unitOfWork.Cars.GetEditMethodAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CarViewModel model)
        {
            string imagesFolderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images/Cars");

            await _unitOfWork.Cars.PostDeleteMethodAsync(model, imagesFolderPath);
            await _unitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}
