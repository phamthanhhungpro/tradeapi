using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trade.InfraModel.DataAccess;

namespace trade.Logic.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryDto Category { get; set; }
    }
}