﻿using trade.Shared.Model.Dtos;
using trade.Logic.Request;
using trade.InfraModel.DataAccess;
using trade.Logic.DTOs;

namespace trade.Logic.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetListCategoryAsync();
        Task<IEnumerable<CategoryDto>> FullListCategoryAsync();
        Task<CudResponseDto> AddCategoryAsync(CategoryRequest request);
        Task<CudResponseDto> EditCategoryAsync(Guid id, CategoryRequest categoryRequest);
        Task<CudResponseDto> DeleteCategoryAsync(Guid id);
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
    }
}