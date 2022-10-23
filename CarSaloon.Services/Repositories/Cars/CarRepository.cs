using CarSaloon.Data.Database;
using CarSaloon.Data.Entities;
using CarSaloon.Services.Repositories.Generic;
using CarSaloon.Services.Services.Utility;
using CarSaloon.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Repositories.Cars
{
    public class CarRepository:GenericRepository<Car>,ICarRepository
    {
        public CarRepository(DatabaseContext context):base(context)
        {

        }

        public async Task<Pagination<CarViewModel>> GetIndexMethodAsync()
        {
            List<Car> allcars=await _context.Cars
                .Include(e=>e.Manufacturer)
                .Include(e=>e.Category)
                .AsNoTracking()
                .ToListAsync();

            List<CarViewModel> viewModelList = new();

            foreach (var car in allcars)
            {
                CarViewModel viewModelItem = new();

                viewModelItem.Id=car.Id;
                viewModelItem.Code=car.Code;
                viewModelItem.Name=car.Name;
                viewModelItem.CarModel=car.Model;
                viewModelItem.Manufacturer = car.Manufacturer.Name;
                viewModelItem.Category = car.Category.Name;
                viewModelItem.Engine = car.Engine;
                viewModelItem.Length = car.Length;
                viewModelItem.Width = car.Width;
                viewModelItem.Color= car.Color;
                viewModelItem.ImageString=car.ImageString;

                viewModelList.Add(viewModelItem);
            }

            return PaginationUtility<CarViewModel>.GetPagination(1, in viewModelList,4);
        }

        public async Task<Pagination<CarViewModel>> PostIndexMethodAsync(string searchText,string manufacturer,string category, int pageIndex,int pageSize)
        {
            IEnumerable<Car> allcars = await _context.Cars
                .Include(e => e.Manufacturer)
                .Include(e => e.Category)
                .AsNoTracking()
                .ToListAsync();

            if(searchText!=null && searchText != string.Empty)
            {
                allcars=allcars.Where(e=>e.Name.ToLower().Contains(searchText.ToLower())
                    || e.Model.ToLower().Contains(searchText.ToLower()))
                    .ToList();
            }

            if(manufacturer!=null && manufacturer != string.Empty)
            {
                allcars = allcars.Where(e => e.Manufacturer == _context.Manufacturers.FirstOrDefault(e => e.Name == manufacturer));
            }

            if(category!=null && category != string.Empty)
            {
                allcars = allcars.Where(e => e.Category == _context.Categories.FirstOrDefault(e => e.Name == category));
            }

            List<CarViewModel> viewModelList = new();

            foreach (var car in allcars)
            {
                CarViewModel viewModelItem = new();

                viewModelItem.Id = car.Id;
                viewModelItem.Code = car.Code;
                viewModelItem.Name = car.Name;
                viewModelItem.CarModel = car.Model;
                viewModelItem.Manufacturer = car.Manufacturer.Name;
                viewModelItem.Category = car.Category.Name;
                viewModelItem.Engine = car.Engine;
                viewModelItem.Length = car.Length;
                viewModelItem.Width = car.Width;
                viewModelItem.Color = car.Color;
                viewModelItem.ImageString = car.ImageString;

                viewModelList.Add(viewModelItem);
            }

            return PaginationUtility<CarViewModel>.GetPagination(pageIndex, in viewModelList, pageSize);
        }

        public async Task PostCreateMethodAsync(CarViewModel carModel, string imagesFolderPath)
        {
            Car newcar = new();

            newcar.Code= carModel.Code;
            newcar.Name = carModel.Name;
            newcar.Model = carModel.CarModel;
            newcar.Manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(e => e.Name == carModel.Manufacturer);
            newcar.Category = await _context.Categories.FirstOrDefaultAsync(e => e.Name == carModel.Category);
            newcar.Engine = carModel.Engine;
            newcar.Length = carModel.Length;
            newcar.Width = carModel.Width;
            newcar.Color = carModel.Color;
            newcar.ImageString=ImageStore.SaveImage(carModel, imagesFolderPath);

            await CreateAsync(newcar);
        }

        public async Task<CarViewModel> GetEditMethodAsync(int id)
        {
            Car carRecord = await _context.Cars
                .Include(e => e.Manufacturer)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            CarViewModel viewModelItem = new();

            viewModelItem.Id = carRecord.Id;
            viewModelItem.Code = carRecord.Code;
            viewModelItem.Name = carRecord.Name;
            viewModelItem.CarModel = carRecord.Model;
            viewModelItem.Manufacturer = carRecord.Manufacturer.Name;
            viewModelItem.Category = carRecord.Category.Name;
            viewModelItem.Engine = carRecord.Engine;
            viewModelItem.Length = carRecord.Length;
            viewModelItem.Width = carRecord.Width;
            viewModelItem.Color = carRecord.Color;
            viewModelItem.ImageString = carRecord.ImageString;

            return viewModelItem;
        }

        public async Task PostEditMethodAsync(CarViewModel carModel, string imagesFolderPath)
        {
            Car carRecord = await _context.Cars
                .Include(e => e.Manufacturer)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == carModel.Id);

            carRecord.Code = carModel.Code;
            carRecord.Name = carModel.Name;
            carRecord.Model= carModel.CarModel;
            carRecord.Manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(e => e.Name == carModel.Manufacturer);
            carRecord.Category=await _context.Categories.FirstOrDefaultAsync(e=>e.Name == carModel.Category);
            carRecord.Engine = carModel.Engine;
            carRecord.Length = carModel.Length;
            carRecord.Width = carModel.Width;
            carRecord.Color = carModel.Color;
            if (carModel.Image != null)
            {
                if (System.IO.File.Exists(Path.Combine(imagesFolderPath, carRecord.ImageString)))
                {
                    System.IO.File.Delete(Path.Combine(imagesFolderPath, carRecord.ImageString));
                }

                carRecord.ImageString = ImageStore.SaveImage(carModel, imagesFolderPath);
            }

            Edit(in carRecord);
        }

        public async Task PostDeleteMethodAsync(CarViewModel carModel, string imagesFolderPath)
        {
            Car carRecord=await GetByIdAsync(carModel.Id);

            if (carModel.Image != null)
            {
                if (System.IO.File.Exists(Path.Combine(imagesFolderPath, carRecord.ImageString)))
                {
                    System.IO.File.Delete(Path.Combine(imagesFolderPath, carRecord.ImageString));
                }
            }

            Delete(in carRecord);
        }

        public async Task<Dictionary<string,string>> ValidateCarAsync(CarViewModel carModel)
        {
            Dictionary<string, string> errors = new();

            IEnumerable<Car> carList = await GetAllAsync();

            // Edit operation
            if (carModel.Id > 0)
            {
                Car carRecord = await GetByIdAsync(carModel.Id);

                if (carModel.Code != carRecord.Code)
                {
                    if (carList.Select(e => e.Code.ToLower()).Contains(carModel.Code.ToLower()))
                    {
                        errors.Add("Code", "There is already Car with this code in database! Please provide different one!");
                    }
                }

                if (carModel.Name != carRecord.Name)
                {
                    if (carList.Select(e => e.Name.ToLower()).Contains(carModel.Name.ToLower()))
                    {
                        errors.Add("Name", "There is already Car with this name in database! Please provide different one!");
                    }
                }

                if(carModel.Manufacturer=="Select manufacturer")
                {
                    errors.Add("Manufacturer", "Please select manufacturer from list!");
                }

                if(carModel.Category=="Select category")
                {
                    errors.Add("Category", "Please select category from list!");
                }
            }
            // Create operation
            else
            {
                if (carList.Select(e => e.Code.ToLower()).Contains(carModel.Code.ToLower()))
                {
                    errors.Add("Code", "There is already Car with this code in database! Please provide different one!");
                }

                if (carList.Select(e => e.Name.ToLower()).Contains(carModel.Name.ToLower()))
                {
                    errors.Add("Name", "There is already Car with this name in database! Please provide different one!");
                }

                if (carModel.Manufacturer == "Select manufacturer")
                {
                    errors.Add("Manufacturer", "Please select manufacturer from list!");
                }

                if (carModel.Category == "Select category")
                {
                    errors.Add("Category", "Please select category from list!");
                }
            }

            return errors;
        }
    }
}
