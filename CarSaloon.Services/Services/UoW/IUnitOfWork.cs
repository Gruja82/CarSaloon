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
    public interface IUnitOfWork:IDisposable
    {
        public ICarRepository Cars { get; }
        public ICategoryRepository Categories { get; }
        public IManufacturerRepository Manufacturers { get; }
        Task Commit();
    }
}
