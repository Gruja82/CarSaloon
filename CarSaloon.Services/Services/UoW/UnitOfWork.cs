using CarSaloon.Data.Database;
using CarSaloon.Services.Repositories.Cars;
using CarSaloon.Services.Repositories.Categories;
using CarSaloon.Services.Repositories.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Services.UoW
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DatabaseContext _context;
        public ICarRepository Cars { get; }
        public ICategoryRepository Categories { get; }
        public IManufacturerRepository Manufacturers { get; }
        public UnitOfWork(DatabaseContext context, ICarRepository cars,ICategoryRepository categories,IManufacturerRepository manufacturers)
        {
            _context = context;
            Cars = cars;
            Categories = categories;
            Manufacturers = manufacturers;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
