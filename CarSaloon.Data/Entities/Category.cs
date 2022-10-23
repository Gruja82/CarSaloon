using CarSaloon.Data.AbstractEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Data.Entities
{
    public class Category:NameEntity
    {
        public string Description { get; set; } = null;
        public virtual ICollection<Car> Cars { get; set; }
    }
}
