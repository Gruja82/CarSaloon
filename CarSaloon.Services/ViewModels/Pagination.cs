using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.ViewModels
{
    public class Pagination<T>
    {
        public List<T> DataList { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}
