using CarSaloon.Data.AbstractEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Data.Entities
{
    public class Car:NameEntity
    {
        public string Model { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public string Engine { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public string Color { get; set; }
        public string ImageString { get; set; } = null;
        public virtual Manufacturer Manufacturer { get; set; } = null;
        public virtual Category Category { get; set; } = null;
    }
}
