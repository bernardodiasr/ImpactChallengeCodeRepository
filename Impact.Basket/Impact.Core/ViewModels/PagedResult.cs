using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Core.ViewModels
{
    public class PagedResult<T> where T : class, new()
    {
        public List<T> Results { get; set; }
        public Page Page { get; set; }

        public int TotalItems => Results.Count;
    }

    public class Page
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
