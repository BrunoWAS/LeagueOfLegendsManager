using Dapper;
using ApiLeague.Data.Repository.Interfaces;

namespace ApiLeague.Data.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly Database _database;

        public TagRepository(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            using (var connection = _database.GetConnection())
            {
                return await connection.QueryAsync<Tag>("SELECT * FROM Tag");
            }
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Tag>(
                    "SELECT * FROM Tag WHERE id = @Id",
                    new { Id = id });
            }
        }

        public async Task AddAsync(Tag tag)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO Tag (nome) VALUES (@Nome)";
                await connection.ExecuteAsync(query, tag);
            }
        }

        public async Task UpdateAsync(Tag tag)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "UPDATE Tag SET nome = @Nome WHERE id = @Id";
                await connection.ExecuteAsync(query, tag);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "DELETE FROM Tag WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
