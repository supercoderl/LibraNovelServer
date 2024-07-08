using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Parameters
{
    public class RequestParameter<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPagesCount
        {
            get
            {
                if (PageSize == 0) return 0;
                return (int)Math.Ceiling((double)TotalItemsCount / PageSize);
            }
        }
        public int PageIndex { get; set; }

        /// <summary>
        /// page number start from 0
        /// </summary>
        public bool Next => PageIndex < TotalPagesCount;
        public bool Previous => PageIndex > 1;
        public IReadOnlyList<T>? Items { get; set; }
    }
}
