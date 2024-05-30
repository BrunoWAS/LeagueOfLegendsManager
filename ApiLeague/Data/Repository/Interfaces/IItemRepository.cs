﻿namespace ApiLeague.Data.Repository.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item> GetByIdAsync(int id);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(int id);
        Task AddItemTagAsync(ItemTag itemTag);
        Task AddItemStatsAsync(ItemStats itemStats);
    }
}
