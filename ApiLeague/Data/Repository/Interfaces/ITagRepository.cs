namespace ApiLeague.Data.Repository.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(int id);
    }
}
