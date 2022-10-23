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

namespace CarSaloon.Services.Repositories.Manufacturers
{
    public class ManufacturerRepository:GenericRepository<Manufacturer>,IManufacturerRepository
    {
        public ManufacturerRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<Pagination<ManufacturerViewModel>> GetIndexMethodAsync()
        {
            List<ManufacturerViewModel> viewModelList = new();

            foreach (var manufacturer in await GetAllAsync())
            {
                ManufacturerViewModel viewModelItem = new();

                viewModelItem.Id = manufacturer.Id;
                viewModelItem.Code = manufacturer.Code;
                viewModelItem.Name = manufacturer.Name;
                viewModelItem.Contact = manufacturer.Contact;
                viewModelItem.Address = manufacturer.Address;
                viewModelItem.City = manufacturer.City;
                viewModelItem.Postal = manufacturer.Postal;
                viewModelItem.Country = manufacturer.Country;
                viewModelItem.Phone = manufacturer.Phone;
                viewModelItem.Email = manufacturer.Email;
                viewModelItem.Web = manufacturer.Web;
                viewModelItem.ImageString = manufacturer.ImageString;

                viewModelList.Add(viewModelItem);
            }

            return PaginationUtility<ManufacturerViewModel>.GetPagination(1, in viewModelList, 4);
        }

        public async Task<Pagination<ManufacturerViewModel>> PostIndexMethodAsync(string searchText, int pageIndex, int pageSize)
        {
            IEnumerable<Manufacturer> allManufacturers = await GetAllAsync();

            if (searchText != null && searchText != string.Empty)
            {
                allManufacturers = allManufacturers.Where(e => e.Code.ToLower().Contains(searchText.ToLower())
                    || e.Name.ToLower().Contains(searchText.ToLower()));
            }

            List<ManufacturerViewModel> viewModelList = new();

            foreach (var manufacturer in allManufacturers)
            {
                ManufacturerViewModel viewModelItem = new();

                viewModelItem.Id = manufacturer.Id;
                viewModelItem.Code = manufacturer.Code;
                viewModelItem.Name = manufacturer.Name;
                viewModelItem.Contact = manufacturer.Contact;
                viewModelItem.Address = manufacturer.Address;
                viewModelItem.City = manufacturer.City;
                viewModelItem.Postal = manufacturer.Postal;
                viewModelItem.Country = manufacturer.Country;
                viewModelItem.Phone = manufacturer.Phone;
                viewModelItem.Email = manufacturer.Email;
                viewModelItem.Web = manufacturer.Web;
                viewModelItem.ImageString = manufacturer.ImageString;

                viewModelList.Add(viewModelItem);
            }

            return PaginationUtility<ManufacturerViewModel>.GetPagination(1, in viewModelList, pageSize);
        }

        public async Task<ManufacturerViewModel> GetEditMethodAsync(int id)
        {
            Manufacturer manufacturerRecord = await GetByIdAsync(id);

            ManufacturerViewModel viewModelItem = new();

            viewModelItem.Id = manufacturerRecord.Id;
            viewModelItem.Code = manufacturerRecord.Code;
            viewModelItem.Name = manufacturerRecord.Name;
            viewModelItem.Contact = manufacturerRecord.Contact;
            viewModelItem.Address = manufacturerRecord.Address;
            viewModelItem.City = manufacturerRecord.City;
            viewModelItem.Postal = manufacturerRecord.Postal;
            viewModelItem.Country = manufacturerRecord.Country;
            viewModelItem.Phone = manufacturerRecord.Phone;
            viewModelItem.Email = manufacturerRecord.Email;
            viewModelItem.Web = manufacturerRecord.Web;
            viewModelItem.ImageString = manufacturerRecord.ImageString;

            return viewModelItem;
        }

        public async Task PostCreateMethodAsync(ManufacturerViewModel model, string imagesFolderPath)
        {
            Manufacturer newManufacturer = new();

            newManufacturer.Code = model.Code;
            newManufacturer.Name = model.Name;
            newManufacturer.Contact = model.Contact;
            newManufacturer.Address = model.Address;
            newManufacturer.City = model.City;
            newManufacturer.Postal = model.Postal;
            newManufacturer.Country = model.Country;
            newManufacturer.Phone = model.Phone;
            newManufacturer.Email = model.Email;
            newManufacturer.Web = model.Web;
            newManufacturer.ImageString = ImageStore.SaveImage(model, imagesFolderPath);

            await CreateAsync(newManufacturer);
        }

        public async Task PostEditMethodAsync(ManufacturerViewModel model, string imagesFolderPath)
        {
            Manufacturer manufacturerRecord = await GetByIdAsync(model.Id);

            manufacturerRecord.Code = model.Code;
            manufacturerRecord.Name = model.Name;
            manufacturerRecord.Contact = model.Contact;
            manufacturerRecord.Address = model.Address;
            manufacturerRecord.City = model.City;
            manufacturerRecord.Postal = model.Postal;
            manufacturerRecord.Country = model.Country;
            manufacturerRecord.Phone = model.Phone;
            manufacturerRecord.Email = model.Email;
            manufacturerRecord.Web = model.Web;
            if (model.Image != null)
            {
                if (System.IO.File.Exists(Path.Combine(imagesFolderPath, manufacturerRecord.ImageString)))
                {
                    System.IO.File.Delete(Path.Combine(imagesFolderPath, manufacturerRecord.ImageString));
                }

                manufacturerRecord.ImageString = ImageStore.SaveImage(model, imagesFolderPath);
            }

            Edit(in manufacturerRecord);
        }

        public async Task PostDeleteMethodAsync(ManufacturerViewModel model, string imagesFolderPath)
        {
            Manufacturer manufacturerRecord = await GetByIdAsync(model.Id);

            if (model.Image != null)
            {
                if (System.IO.File.Exists(Path.Combine(imagesFolderPath, manufacturerRecord.ImageString)))
                {
                    System.IO.File.Delete(Path.Combine(imagesFolderPath, manufacturerRecord.ImageString));
                }
            }

            Delete(in manufacturerRecord);
        }

        public async Task<Dictionary<string, string>> ValidateManufacturerAsync(ManufacturerViewModel model)
        {
            Dictionary<string, string> errors = new();

            IEnumerable<Manufacturer> manufacturerList = await GetAllAsync();

            // Edit operation
            if (model.Id > 0)
            {
                Manufacturer manufacturerRecord = await GetByIdAsync(model.Id);

                if (model.Code != manufacturerRecord.Code)
                {
                    if (manufacturerList.Select(e => e.Code.ToLower()).Contains(model.Code.ToLower()))
                    {
                        errors.Add("Code", "There is already Manufacturer with this code in database! Please provide different one!");
                    }
                }

                if (model.Name != manufacturerRecord.Name)
                {
                    if (manufacturerList.Select(e => e.Name.ToLower()).Contains(model.Name.ToLower()))
                    {
                        errors.Add("Name", "There is already Manufacturer with this name in database! Please provide different one!");
                    }
                }
            }
            // Create operation
            else
            {
                if (manufacturerList.Select(e => e.Code.ToLower()).Contains(model.Code.ToLower()))
                {
                    errors.Add("Code", "There is already Manufacturer with this code in database! Please provide different one!");
                }

                if (manufacturerList.Select(e => e.Name.ToLower()).Contains(model.Name.ToLower()))
                {
                    errors.Add("Name", "There is already Manufacturer with this name in database! Please provide different one!");
                }
            }

            return errors;
        }
    }
}
