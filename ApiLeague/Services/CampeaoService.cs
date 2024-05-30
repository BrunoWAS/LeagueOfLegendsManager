using ApiLeague.Data.Repository.Interfaces;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Services
{
    public class CampeaoService : ICampeaoService
    {
        private readonly ICampeaoRepository _campeaoRepository;

        public CampeaoService(ICampeaoRepository campeaoRepository)
        {
            _campeaoRepository = campeaoRepository;
        }

        public Task<IEnumerable<Campeao>> GetAllAsync()
        {
            return _campeaoRepository.GetAllAsync();
        }

        public Task<Campeao> GetByIdAsync(int id)
        {
            return _campeaoRepository.GetByIdAsync(id);
        }

        public Task AddAsync(Campeao campeao)
        {
            return _campeaoRepository.AddAsync(campeao);
        }

        public Task UpdateAsync(Campeao campeao)
        {
            return _campeaoRepository.UpdateAsync(campeao);
        }

        public Task DeleteAsync(int id)
        {
            return _campeaoRepository.DeleteAsync(id);
        }
    }
}
