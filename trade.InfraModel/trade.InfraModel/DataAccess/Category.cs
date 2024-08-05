using trade.Shared.Model.BaseModel;

namespace trade.InfraModel.DataAccess
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public ICollection<Store> Stores { get; set; }
    }
}