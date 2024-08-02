using trade.Shared.Model.Dtos;
using trade.Logic.Request;
using trade.Logic.DTOs;

namespace trade.Logic.Interfaces
{
    public interface IProductService
    {
        Task<CudResponseDto> AddProductAsync(ProductRequest productRequest);
        Task<IEnumerable<ProductDto>> GetListProductAsync();
        Task<CudResponseDto> EditProductAsync(Guid id, ProductRequest productRequest);
        Task<CudResponseDto> DeleteProductAsync(Guid id);
        Task<ProductDto> GetProductByIdAsync(Guid id);
    }
}