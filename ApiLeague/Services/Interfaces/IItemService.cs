namespace ApiLeague.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item> GetByIdAsync(int id);
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(int id);
    }
}
