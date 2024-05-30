using Dapper;
using ApiLeague.Data.Repository.Interfaces;

namespace ApiLeague.Data.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly Database _database;

        public ItemRepository(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            using (var connection = _database.GetConnection())
            {
                var items = await connection.QueryAsync<Item>("SELECT * FROM Item");

                foreach (var item in items)
                {
                    item.Tags = (await connection.QueryAsync<Tag>(
                        "SELECT t.* FROM Tag t JOIN ItemTag it ON t.id = it.tag_id WHERE it.item_id = @ItemId",
                        new { ItemId = item.Id })).AsList();

                    item.Stats = (await connection.QueryAsync<ItemStats>(
                        "SELECT * FROM ItemStats WHERE item_id = @ItemId",
                        new { ItemId = item.Id })).AsList();
                }

                return items;
            }
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                var item = await connection.QuerySingleOrDefaultAsync<Item>(
                    "SELECT * FROM Item WHERE id = @Id",
                    new { Id = id });

                if (item != null)
                {
                    item.Tags = (await connection.QueryAsync<Tag>(
                        "SELECT t.* FROM Tag t JOIN ItemTag it ON t.id = it.tag_id WHERE it.item_id = @ItemId",
                        new { ItemId = item.Id })).AsList();

                    item.Stats = (await connection.QueryAsync<ItemStats>(
                        "SELECT * FROM ItemStats WHERE item_id = @ItemId",
                        new { ItemId = item.Id })).AsList();
                }

                return item;
            }
        }

        public async Task AddAsync(Item item)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO Item (nome, custo) VALUES (@Nome, @Custo)";
                var itemId = await connection.ExecuteScalarAsync<int>(query, item);

                foreach (var tag in item.Tags)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO ItemTag (item_id, tag_id) VALUES (@ItemId, @TagId)",
                        new { ItemId = itemId, TagId = tag.Id });
                }

                foreach (var stat in item.Stats)
                {
                    stat.ItemId = itemId;
                    await connection.ExecuteAsync(
                        "INSERT INTO ItemStats (item_id, stat_nome, stat_valor) VALUES (@ItemId, @StatNome, @StatValor)",
                        stat);
                }
            }
        }

        public async Task UpdateAsync(Item item)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "UPDATE Item SET nome = @Nome, custo = @Custo WHERE id = @Id";
                await connection.ExecuteAsync(query, item);

                await connection.ExecuteAsync(
                    "DELETE FROM ItemTag WHERE item_id = @ItemId",
                    new { ItemId = item.Id });

                foreach (var tag in item.Tags)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO ItemTag (item_id, tag_id) VALUES (@ItemId, @TagId)",
                        new { ItemId = item.Id, TagId = tag.Id });
                }

                await connection.ExecuteAsync(
                    "DELETE FROM ItemStats WHERE item_id = @ItemId",
                    new { ItemId = item.Id });

                foreach (var stat in item.Stats)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO ItemStats (item_id, stat_nome, stat_valor) VALUES (@ItemId, @StatNome, @StatValor)",
                        stat);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _database.GetConnection())
            {
                await connection.ExecuteAsync(
                    "DELETE FROM ItemTag WHERE item_id = @ItemId",
                    new { ItemId = id });

                await connection.ExecuteAsync(
                    "DELETE FROM ItemStats WHERE item_id = @ItemId",
                    new { ItemId = id });

                var query = "DELETE FROM Item WHERE id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task AddItemTagAsync(ItemTag itemTag)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO ItemTag (item_id, tag_id) VALUES (@ItemId, @TagId)";
                await connection.ExecuteAsync(query, itemTag);
            }
        }

        public async Task AddItemStatsAsync(ItemStats itemStats)
        {
            using (var connection = _database.GetConnection())
            {
                var query = "INSERT INTO ItemStats (item_id, stat_nome, stat_valor) VALUES (@ItemId, @StatNome, @StatValor)";
                await connection.ExecuteAsync(query, itemStats);
            }
        }

    }
}
