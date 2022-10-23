using CarSaloon.Data.Database;
using CarSaloon.Services.Repositories.Cars;
using CarSaloon.Services.Repositories.Categories;
using CarSaloon.Services.Repositories.Manufacturers;
using CarSaloon.Services.Services.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Services.Utility
{
    public static class RegisterServices
    {
        public static void AddServices(IServiceCollection services,string connection)
        {
            services.AddDbContext<DatabaseContext>(options=>options.UseSqlServer(connection));
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }
    }
}
