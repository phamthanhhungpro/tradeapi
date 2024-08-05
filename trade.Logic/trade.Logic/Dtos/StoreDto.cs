using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trade.InfraModel.DataAccess;

namespace trade.Logic.Dtos
{
    public class StoreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int Sold { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public IList<Product> Products { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}
