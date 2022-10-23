using CarSaloon.Data.Entities;
using CarSaloon.Services.Repositories.Generic;
using CarSaloon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Repositories.Manufacturers
{
    public interface IManufacturerRepository:IGenericRepository<Manufacturer>
    {
        Task<Dictionary<string, string>> ValidateManufacturerAsync(ManufacturerViewModel model);
        Task<Pagination<ManufacturerViewModel>> GetIndexMethodAsync();
        Task<Pagination<ManufacturerViewModel>> PostIndexMethodAsync(string searchText,int pageIndex,int pageSize);
        Task PostCreateMethodAsync(ManufacturerViewModel model,string imagesFolderPath);
        Task<ManufacturerViewModel> GetEditMethodAsync(int id);
        Task PostEditMethodAsync(ManufacturerViewModel model,string imagesFolderPath);
        Task PostDeleteMethodAsync(ManufacturerViewModel model, string imagesFolderPath);
    }
}
