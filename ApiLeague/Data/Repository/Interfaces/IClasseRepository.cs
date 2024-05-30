namespace ApiLeague.Data.Repository.Interfaces
{
    public interface IClasseRepository
    {
        Task<IEnumerable<Classe>> GetAllAsync();
        Task<Classe> GetByIdAsync(int id);
        Task AddAsync(Classe classe);
        Task UpdateAsync(Classe classe);
        Task DeleteAsync(int id);
    }
}
