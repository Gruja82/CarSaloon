using CarSaloon.Data.Entities;
using CarSaloon.Services.Repositories.Generic;
using CarSaloon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Repositories.Cars
{
    public interface ICarRepository:IGenericRepository<Car>
    {
        Task<Dictionary<string, string>> ValidateCarAsync(CarViewModel carModel);
        Task<Pagination<CarViewModel>> GetIndexMethodAsync();
        Task<Pagination<CarViewModel>> PostIndexMethodAsync(string searchText,string manufacturer,string category,int pageIndex,int pageSize);
        Task PostCreateMethodAsync(CarViewModel carModel, string imagesFolderPath);
        Task<CarViewModel> GetEditMethodAsync(int id);
        Task PostEditMethodAsync(CarViewModel carModel, string imagesFolderPath);
        Task PostDeleteMethodAsync(CarViewModel carModel, string imagesFolderPath);
    }
}
