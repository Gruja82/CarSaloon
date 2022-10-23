using CarSaloon.Data.AbstractEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Data.Entities
{
    public class Manufacturer:NameEntity
    {
        public string Contact { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Web { get; set; } = null;
        public string ImageString { get; set; } = null;
        public virtual ICollection<Car> Cars { get; set; }
    }
}
