using CarSaloon.Data.Database;
using CarSaloon.Data.Entities;
using CarSaloon.Services.Repositories.Generic;
using CarSaloon.Services.Services.Utility;
using CarSaloon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Repositories.Categories
{
    public class CategoryRepository:GenericRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(DatabaseContext context):base(context)
        {

        }

        public async Task<Pagination<CategoryViewModel>> GetIndexMethodAsync()
        {
            List<CategoryViewModel> viewModelList = new();

            foreach (var category in await GetAllAsync())
            {
                CategoryViewModel viewModelItem = new();

                viewModelItem.Id= category.Id;
                viewModelItem.Code= category.Code;
                viewModelItem.Name= category.Name;
                viewModelItem.Description= category.Description;

                viewModelList.Add(viewModelItem);
            }

            return PaginationUtility<CategoryViewModel>.GetPagination(1, in viewModelList, 4);
        }

        public async Task<Pagination<CategoryViewModel>> PostIndexMethodAsync(string searchText,int pageIndex,int pageSize)
        {
            IEnumerable<Category> allCategories=await GetAllAsync();

            if(searchText!=null && searchText != string.Empty)
            {
                allCategories=allCategories.Where(e=>e.Code.ToLower().Contains(searchText.ToLower())
                    || e.Name.ToLower().Contains(searchText.ToLower()));
            }

            List<CategoryViewModel> viewModelList = new();

            foreach (var category in await GetAllAsync())
            {
                CategoryViewModel viewModelItem = new();

                viewModelItem.Id = category.Id;
                viewModelItem.Code = category.Code;
                viewModelItem.Name = category.Name;
                viewModelItem.Description = category.Description;

                viewModelList.Add(viewModelItem);
            }

            return PaginationUtility<CategoryViewModel>.GetPagination(pageIndex, in viewModelList, pageSize);
        }

        public async Task PostCreateMethodAsync(CategoryViewModel model)
        {
            Category newCategory = new();

            newCategory.Code = model.Code;
            newCategory.Name = model.Name;
            newCategory.Description = model.Description;

            await CreateAsync(newCategory);
        }

        public async Task<CategoryViewModel> GetEditMethodAsync(int id)
        {
            Category categoryRecord = await GetByIdAsync(id);

            CategoryViewModel viewModelItem = new();

            viewModelItem.Id=categoryRecord.Id;
            viewModelItem.Code = categoryRecord.Code;
            viewModelItem.Name=categoryRecord.Name;
            viewModelItem.Description = categoryRecord.Description;

            return viewModelItem;
        }

        public async Task PostEditMethodAsync(CategoryViewModel model)
        {
            Category categoryRecord = await GetByIdAsync(model.Id);

            categoryRecord.Code=model.Code;
            categoryRecord.Name=model.Name;
            categoryRecord.Description=model.Description;

            Edit(in categoryRecord);
        }

        public async Task PostDeleteMethodAsync(CategoryViewModel model)
        {
            Category categoryRecord=await GetByIdAsync(model.Id);

            Delete(in categoryRecord);
        }

        public async Task<Dictionary<string,string>> ValidateCategoryAsync(CategoryViewModel model)
        {
            Dictionary<string, string> errors = new();

            IEnumerable<Category> categoryList=await GetAllAsync();

            // Edit operation
            if (model.Id > 0)
            {
                Category categoryRecord = await GetByIdAsync(model.Id);

                if (model.Code != categoryRecord.Code)
                {
                    if (categoryList.Select(e => e.Code.ToLower()).Contains(model.Code))
                    {
                        errors.Add("Code", "There is already Category with this code in database! Please provide different one!");
                    }
                }

                if (model.Name != categoryRecord.Name)
                {
                    if (categoryList.Select(e => e.Name.ToLower()).Contains(model.Name))
                    {
                        errors.Add("Name", "There is already Category with this name in database! Please provide different one!");
                    }
                }
            }
            // Create operation
            else
            {
                if (categoryList.Select(e => e.Code.ToLower()).Contains(model.Code))
                {
                    errors.Add("Code", "There is already Category with this code in database! Please provide different one!");
                }

                if (categoryList.Select(e => e.Name.ToLower()).Contains(model.Name))
                {
                    errors.Add("Name", "There is already Category with this name in database! Please provide different one!");
                }
            }

            return errors;
        }
    }
}
