using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;
using trade.Logic.DTOs;
using trade.Logic.Interfaces;
using trade.Logic.Request;
using trade.Shared.Model.Dtos;

namespace trade.Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CudResponseDto> AddProductAsync(ProductRequest productRequest)
        {
            var category = await _context.Categories.FindAsync(productRequest.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            var product = new Product()
            {
                Name = productRequest.ProductName,
                Category = category,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return new CudResponseDto()
            {
                Id = product.Id,
            };
        }

        public async Task<CudResponseDto> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new CudResponseDto
            {
                Id = product.Id,
            };
        }

        public async Task<CudResponseDto> EditProductAsync(Guid id, ProductRequest productRequest)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            var category = await _context.Categories.FindAsync(productRequest.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            product.Name = productRequest.ProductName;
            product.Category = category;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new CudResponseDto
            {
                Id = product.Id,
            };
        }

        public async Task<IEnumerable<ProductDto>> GetListProductAsync()
        {
            return await _context.Products
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .Select(x => new ProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = new CategoryDto()
                    {
                        Id = x.CategoryId,
                        Name = x.Category.CategoryName
                    },
                }).ToListAsync();
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = new CategoryDto ()
                { 
                    Id = product.CategoryId,
                    Name = product.Category.CategoryName
                },
            };
        }
    }
}