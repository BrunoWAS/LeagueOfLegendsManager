using Dapper;
using ApiLeague.Data.Repository.Interfaces;

namespace ApiLeague.Data.Repository
{
    public class ClasseRepository : IClasseRepository
    {
        private readonly Database _database;

        public ClasseRepository(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Classe>> GetAllAsync()
        {
            using (var connection = _database.GetConnection())
            {
                return await connection.QueryAsync<Classe>("SELECT * FROM Classe");
            }
        }

        public async Task<Classe> GetByIdAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Classe>(
                    "SELECT * FROM Classe WHERE id = @Id",
                    new { Id = id });
            }
        }

        public async Task AddAsync(Classe classe)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO Classe (nome) VALUES (@Nome)";
                await connection.ExecuteAsync(query, classe);
            }
        }

        public async Task UpdateAsync(Classe classe)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "UPDATE Classe SET nome = @Nome WHERE id = @Id";
                await connection.ExecuteAsync(query, classe);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "DELETE FROM Classe WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
