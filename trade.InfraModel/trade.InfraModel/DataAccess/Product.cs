using trade.Shared.Model.BaseModel;

namespace trade.InfraModel.DataAccess
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
