using CarSaloon.Services.Services.UoW;
using CarSaloon.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarSaloon.Mvc.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ManufacturerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Manufacturers.GetIndexMethodAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchText,int pageIndex,int pageSize)
        {
            return View(await _unitOfWork.Manufacturers.PostIndexMethodAsync(searchText,pageIndex,pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManufacturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = await _unitOfWork.Manufacturers.ValidateManufacturerAsync(model);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(model);
                }

                string imagesFolderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images/Manufacturers");

                await _unitOfWork.Manufacturers.PostCreateMethodAsync(model,imagesFolderPath);
                await _unitOfWork.Commit();
                int lastManufacturerId = _unitOfWork.Manufacturers.GetAllAsync().Result.LastOrDefault().Id;
                return RedirectToAction(nameof(Edit), new { id = lastManufacturerId });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _unitOfWork.Manufacturers.GetEditMethodAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManufacturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = await _unitOfWork.Manufacturers.ValidateManufacturerAsync(model);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(model);
                }

                string imagesFolderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images/Manufacturers");

                await _unitOfWork.Manufacturers.PostEditMethodAsync(model, imagesFolderPath);
                await _unitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _unitOfWork.Manufacturers.GetEditMethodAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ManufacturerViewModel model)
        {
            string imagesFolderPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images/Manufacturers");

            await _unitOfWork.Manufacturers.PostDeleteMethodAsync(model, imagesFolderPath);
            await _unitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}
