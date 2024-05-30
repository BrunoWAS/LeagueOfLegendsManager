namespace ApiLeague.Data.Repository.Interfaces
{
    public interface ICampeaoRepository
    {
        Task<IEnumerable<Campeao>> GetAllAsync();
        Task<Campeao> GetByIdAsync(int id);
        Task AddAsync(Campeao campeao);
        Task UpdateAsync(Campeao campeao);
        Task DeleteAsync(int id);
        Task AddCampeaoClasseAsync(CampeaoClasse campeaoClasse);
    }
}
