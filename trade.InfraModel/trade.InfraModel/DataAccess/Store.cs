using trade.Shared.Model.BaseModel;

namespace trade.InfraModel.DataAccess
{
    public class Store : BaseEntity
    {
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
