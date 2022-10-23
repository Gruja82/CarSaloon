using CarSaloon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Services.Utility
{
    public static class PaginationUtility<T> where T : class
    {
        public static Pagination<T> GetPagination(int currentPage,in List<T> dataList,int pageSize)
        {
            Pagination<T> pagination = new();

            pagination.DataList=(from c in dataList select c)
                .Skip((currentPage-1) * pageSize)
                .Take(pageSize)
                .ToList();

            double pageCount = (double)((decimal)dataList.Count / Convert.ToDecimal(pageSize));

            pagination.PageCount=(int)Math.Ceiling(pageCount);

            pagination.CurrentPageIndex=currentPage;

            pagination.PageSize=pageSize;

            return pagination;
        }
    }
}
