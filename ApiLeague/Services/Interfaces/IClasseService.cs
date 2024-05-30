namespace ApiLeague.Services.Interfaces
{
    public interface IClasseService
    {
        Task<IEnumerable<Classe>> GetAllAsync();
        Task<Classe> GetByIdAsync(int id);
        Task AddAsync(Classe classe);
        Task UpdateAsync(Classe classe);
        Task DeleteAsync(int id);
    }
}
