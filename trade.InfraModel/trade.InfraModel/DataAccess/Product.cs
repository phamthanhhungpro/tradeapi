using trade.Shared.Model.BaseModel;

namespace trade.InfraModel.DataAccess
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SoLuong { get; set; }

        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}
