using CarSaloon.Data.Entities;
using CarSaloon.Services.Repositories.Generic;
using CarSaloon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Repositories.Categories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<Dictionary<string, string>> ValidateCategoryAsync(CategoryViewModel model);
        Task<Pagination<CategoryViewModel>> GetIndexMethodAsync();
        Task<Pagination<CategoryViewModel>> PostIndexMethodAsync(string searchText, int pageIndex, int pageSize);
        Task PostCreateMethodAsync(CategoryViewModel model);
        Task<CategoryViewModel> GetEditMethodAsync(int id);
        Task PostEditMethodAsync(CategoryViewModel model);
        Task PostDeleteMethodAsync(CategoryViewModel model);
    }
}
