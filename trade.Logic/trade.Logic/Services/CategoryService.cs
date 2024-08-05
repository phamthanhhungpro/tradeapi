using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;
using trade.Shared.Model.Dtos;
using trade.Logic.DTOs;
using trade.Logic.Interfaces;
using trade.Logic.Request;
using trade.Shared.Dtos;

namespace trade.Logic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CudResponseDto> AddCategoryAsync(CategoryRequest request)
        {
            var category = new Category()
            {
                CategoryName = request.Name,
                CreatedAt = DateTime.UtcNow,
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return new CudResponseDto()
            {
                Id = category.Id,
            };
        }

        public async Task<CudResponseDto> DeleteCategoryAsync(Guid id)
        {
            var item = await _dbContext.Categories.FindAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }
            item.IsDeleted = true;
            item.DeletedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return new CudResponseDto()
            {
                Id = item.Id,
            };
        }

        public async Task<CudResponseDto> EditCategoryAsync(Guid id, CategoryRequest categoryRequest)
        {
            var item = await _dbContext.Categories.FindAsync(id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }
            item.CategoryName = categoryRequest.Name;
            item.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return new CudResponseDto()
            {
                Id = item.Id,
            };
        }


        public async Task<IEnumerable<Category>> GetListCategoryAsync()
        {
            return await _dbContext.Categories
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<PagingResponse<CategoryDto>> GetPagingCategoryAsync(PagingRequest pagingRequest)
        {
            var categories = await _dbContext.Categories
                            .OrderByDescending(x => x.CreatedAt)
                            .Skip(pagingRequest.PageSize * pagingRequest.PageIndex)
                            .Take(pagingRequest.PageSize)
                            .AsNoTracking()
                            .Select(x => new CategoryDto()
                            {
                                Id = x.Id,
                                Name = x.CategoryName
                            }).ToListAsync();

            var totalRecords = await _dbContext.Categories.CountAsync();

            return new PagingResponse<CategoryDto>()
            {
                Items = categories,
                Count = totalRecords,
            };
        }
    }
}