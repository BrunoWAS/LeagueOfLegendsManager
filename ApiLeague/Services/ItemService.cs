using ApiLeague.Data.Repository.Interfaces;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public Task<IEnumerable<Item>> GetAllAsync()
        {
            return _itemRepository.GetAllAsync();
        }

        public Task<Item> GetByIdAsync(int id)
        {
            return _itemRepository.GetByIdAsync(id);
        }

        public Task AddAsync(Item item)
        {
            return _itemRepository.AddAsync(item);
        }

        public Task UpdateAsync(Item item)
        {
            return _itemRepository.UpdateAsync(item);
        }

        public Task DeleteAsync(int id)
        {
            return _itemRepository.DeleteAsync(id);
        }
    }
}
