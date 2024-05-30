namespace ApiLeague.Services.Interfaces
{
    public interface ICampeaoService
    {
        Task<IEnumerable<Campeao>> GetAllAsync();
        Task<Campeao> GetByIdAsync(int id);
        Task AddAsync(Campeao campeao);
        Task UpdateAsync(Campeao campeao);
        Task DeleteAsync(int id);
    }
}
