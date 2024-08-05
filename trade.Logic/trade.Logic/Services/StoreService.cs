using Microsoft.EntityFrameworkCore;
using trade.InfraModel.DataAccess;
using trade.Logic.Dtos;
using trade.Logic.Requests;
using trade.Shared.Dtos;
using trade.Shared.Model.Dtos;

namespace trade.Logic.Services
{
    public interface IStoreService
    {
        Task<CudResponseDto> CreateStoreAsync(CreateStoreRequest storeRequest);
        Task<CudResponseDto> DeleteStoreAsync(Guid id);
        Task<PagingResponse<StoreDto>> GetStoreAsync(PagingRequest request);
    }

    public class StoreService : IStoreService
    {
        private readonly AppDbContext _context;
        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CudResponseDto> CreateStoreAsync(CreateStoreRequest storeRequest)
        {
            var store = new Store()
            {
                Name = storeRequest.Name,
                Description = storeRequest.Description,
                CreatedAt = DateTime.UtcNow,
                Image = storeRequest.Image,
                CategoryId = storeRequest.CategoryId,
                UserId = storeRequest.UserId
            };

            await _context.Stores.AddAsync(store);

            await _context.SaveChangesAsync();
            return new CudResponseDto()
            {
                Id = store.Id,
                IsSucceeded = true
            };

        }

        public async Task<CudResponseDto> DeleteStoreAsync(Guid id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return new CudResponseDto
                {
                    IsSucceeded = false,
                    Message = "Store not found"
                };
            }
            store.IsDeleted = true;
            store.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new CudResponseDto
            {
                Id = store.Id,
                IsSucceeded = true
            };
        }

        public async Task<PagingResponse<StoreDto>> GetStoreAsync(PagingRequest request)
        {
            var stores = await _context.Stores
                .OrderByDescending(s => s.CreatedAt)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .Include(s => s.Category)
                .Include(s => s.User)
                .Include(s => s.Products)
                .Include(s => s.Comments)
                .Select(s => new StoreDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Image = s.Image,
                    CategoryId = s.CategoryId,
                    UserId = s.UserId,
                    Category = s.Category,
                    User = s.User,
                    Products = s.Products,
                    Comments = s.Comments
                })
                .ToListAsync();

            var totalRecords = await _context.Stores.CountAsync();

            return new PagingResponse<StoreDto>
            {
                Items = stores,
                Count = totalRecords
            };
        }
    }
}
