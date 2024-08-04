using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trade.Shared.Dtos
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PagingRequest()
        {
            PageIndex = 0;
            PageSize = 50;
        }
    }
}
