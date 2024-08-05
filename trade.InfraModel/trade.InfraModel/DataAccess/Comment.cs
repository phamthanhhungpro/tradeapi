using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trade.Shared.Model.BaseModel;

namespace trade.InfraModel.DataAccess
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public int Rating { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public string CommentedBy { get; set; }
    }
}
