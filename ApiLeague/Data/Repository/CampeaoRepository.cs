using Dapper;
using ApiLeague.Data.Repository.Interfaces;

namespace ApiLeague.Data.Repository
{
    public class CampeaoRepository : ICampeaoRepository
    {
        private readonly Database _database;

        public CampeaoRepository(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Campeao>> GetAllAsync()
        {
            using (var connection = _database.GetConnection())
            {
                var campeoes = await connection.QueryAsync<Campeao>("SELECT * FROM Campeao");

                foreach (var campeao in campeoes)
                {
                    campeao.Classes = (await connection.QueryAsync<Classe>(
                        "SELECT c.* FROM Classe c JOIN CampeaoClasse cc ON c.id = cc.classe_id WHERE cc.campeao_id = @CampeaoId",
                        new { CampeaoId = campeao.Id })).AsList();
                }

                return campeoes;
            }
        }

        public async Task<Campeao> GetByIdAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                var campeao = await connection.QuerySingleOrDefaultAsync<Campeao>(
                    "SELECT * FROM Campeao WHERE id = @Id",
                    new { Id = id });

                if (campeao != null)
                {
                    campeao.Classes = (await connection.QueryAsync<Classe>(
                        "SELECT c.* FROM Classe c JOIN CampeaoClasse cc ON c.id = cc.classe_id WHERE cc.campeao_id = @CampeaoId",
                        new { CampeaoId = campeao.Id })).AsList();
                }

                return campeao;
            }
        }

        public async Task AddAsync(Campeao campeao)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO Campeao (nome, title) VALUES (@Nome, @Title)";
                var campeaoId = await connection.ExecuteScalarAsync<int>(query, campeao);

                foreach (var classe in campeao.Classes)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO CampeaoClasse (campeao_id, classe_id) VALUES (@CampeaoId, @ClasseId)",
                        new { CampeaoId = campeaoId, ClasseId = classe.Id });
                }
            }
        }

        public async Task UpdateAsync(Campeao campeao)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "UPDATE Campeao SET nome = @Nome, title = @Title WHERE id = @Id";
                await connection.ExecuteAsync(query, campeao);

                await connection.ExecuteAsync(
                    "DELETE FROM CampeaoClasse WHERE campeao_id = @CampeaoId",
                    new { CampeaoId = campeao.Id });

                foreach (var classe in campeao.Classes)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO CampeaoClasse (campeao_id, classe_id) VALUES (@CampeaoId, @ClasseId)",
                        new { CampeaoId = campeao.Id, ClasseId = classe.Id });
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                await connection.ExecuteAsync(
                    "DELETE FROM CampeaoClasse WHERE campeao_id = @CampeaoId",
                    new { CampeaoId = id });

                var query = "DELETE FROM Campeao WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task AddCampeaoClasseAsync(CampeaoClasse campeaoClasse)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO CampeaoClasse (campeao_id, classe_id) VALUES (@CampeaoId, @ClasseId)";
                await connection.ExecuteAsync(query, campeaoClasse);
            }
        }

    }
}
