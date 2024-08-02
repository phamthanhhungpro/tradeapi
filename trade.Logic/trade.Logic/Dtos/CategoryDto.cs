using trade.InfraModel.DataAccess;

namespace trade.Logic.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProductDto> products { get; set; }
    }
}