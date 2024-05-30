using ApiLeague.Data.Repository.Interfaces;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Services
{
    public class ClasseService : IClasseService
    {
        private readonly IClasseRepository _classeRepository;

        public ClasseService(IClasseRepository classeRepository)
        {
            _classeRepository = classeRepository;
        }

        public Task<IEnumerable<Classe>> GetAllAsync()
        {
            return _classeRepository.GetAllAsync();
        }

        public Task<Classe> GetByIdAsync(int id)
        {
            return _classeRepository.GetByIdAsync(id);
        }

        public Task AddAsync(Classe classe)
        {
            return _classeRepository.AddAsync(classe);
        }

        public Task UpdateAsync(Classe classe)
        {
            return _classeRepository.UpdateAsync(classe);
        }

        public Task DeleteAsync(int id)
        {
            return _classeRepository.DeleteAsync(id);
        }
    }
}
