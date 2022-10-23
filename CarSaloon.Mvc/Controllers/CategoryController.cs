using CarSaloon.Services.Services.UoW;
using CarSaloon.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarSaloon.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Categories.GetIndexMethodAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchText,int pageIndex,int pageSize)
        {
            return View(await _unitOfWork.Categories.PostIndexMethodAsync(searchText,pageIndex,pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = await _unitOfWork.Categories.ValidateCategoryAsync(model);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(model);
                }

                await _unitOfWork.Categories.PostCreateMethodAsync(model);
                await _unitOfWork.Commit();
                int lastCategoryId = _unitOfWork.Categories.GetAllAsync().Result.LastOrDefault().Id;
                return RedirectToAction(nameof(Edit), new { id = lastCategoryId });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _unitOfWork.Categories.GetEditMethodAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = await _unitOfWork.Categories.ValidateCategoryAsync(model);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    return View(model);
                }

                await _unitOfWork.Categories.PostEditMethodAsync(model);
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
            return View(await _unitOfWork.Categories.GetEditMethodAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryViewModel model)
        {
            await _unitOfWork.Categories.PostDeleteMethodAsync(model);
            await _unitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}
