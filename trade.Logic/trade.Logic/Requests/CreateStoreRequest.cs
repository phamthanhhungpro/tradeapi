using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trade.InfraModel.DataAccess;

namespace trade.Logic.Requests
{
    public class CreateStoreRequest
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
    }
}
