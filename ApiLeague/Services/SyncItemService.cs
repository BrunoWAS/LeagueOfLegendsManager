using ApiLeague.Data.Repository.Interfaces;
using Newtonsoft.Json.Linq;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Services
{
    public class SyncItemService : ISyncItemService
    {
        private readonly HttpClient _httpClient;
        private readonly ITagRepository _tagRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IConfiguration _configuration;

        public SyncItemService(HttpClient httpClient, ITagRepository tagRepository, IItemRepository itemRepository, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _tagRepository = tagRepository;
            _itemRepository = itemRepository;
            _configuration = configuration;
        }

        public async Task<string> SynchronizeItemsAsync()
        {
            string apiUrl = _configuration["ApiLol-itens"];
            var response = await _httpClient.GetStringAsync(apiUrl);
            var data = JObject.Parse(response)["data"];
            var items = data.Children().Select(c => c.First).ToList();

            var existingTags = (await _tagRepository.GetAllAsync()).ToDictionary(t => t.Nome, t => t.Id);
            var existingItems = (await _itemRepository.GetAllAsync()).ToDictionary(i => i.Nome, i => i.Id);

            var newTags = new List<Tag>();
            var newItems = new List<Item>();
            var itemTags = new List<ItemTag>();
            var itemStats = new List<ItemStats>();

            // Process items and prepare new tags and items
            foreach (var item in items)
            {
                string itemName = item["name"].ToString();
                int itemCost = int.Parse(item["gold"]["total"].ToString());
                var tags = item["tags"].Select(t => t.ToString()).ToList();
                var stats = item["stats"].Children<JProperty>().ToDictionary(stat => stat.Name, stat => decimal.Parse(stat.Value.ToString()));

                // Prepare new tags
                foreach (var tag in tags)
                {
                    if (!existingTags.ContainsKey(tag))
                    {
                        var newTag = new Tag { Nome = tag };
                        newTags.Add(newTag);
                        existingTags[tag] = 0; // Placeholder, will update after insert
                    }
                }

                // Prepare new items
                if (!existingItems.ContainsKey(itemName))
                {
                    var newItem = new Item { Nome = itemName, Custo = itemCost };
                    newItems.Add(newItem);
                    existingItems[itemName] = 0; // Placeholder, will update after insert
                }
            }

            // Insert new tags and update existingTags dictionary
            foreach (var newTag in newTags)
            {
                await _tagRepository.AddAsync(newTag);
                existingTags[newTag.Nome] = newTag.Id;
            }

            // Insert new items and update existingItems dictionary
            foreach (var newItem in newItems)
            {
                await _itemRepository.AddAsync(newItem);
                existingItems[newItem.Nome] = newItem.Id;
            }

            // Prepare relationships and stats
            foreach (var item in items)
            {
                string itemName = item["name"].ToString();
                int itemId = existingItems[itemName];
                var tags = item["tags"].Select(t => t.ToString()).ToList();
                var stats = item["stats"].Children<JProperty>().ToDictionary(stat => stat.Name, stat => decimal.Parse(stat.Value.ToString()));

                foreach (var tag in tags)
                {
                    int tagId = existingTags[tag];
                    if (!itemTags.Any(it => it.ItemId == itemId && it.TagId == tagId))
                    {
                        itemTags.Add(new ItemTag { ItemId = itemId, TagId = tagId });
                    }
                }

                foreach (var stat in stats)
                {
                    if (!itemStats.Any(itemStat => itemStat.ItemId == itemId && itemStat.StatNome == stat.Key))
                    {
                        itemStats.Add(new ItemStats { ItemId = itemId, StatNome = stat.Key, StatValor = stat.Value });
                    }
                }
            }

            // Insert ItemTag relationships
            foreach (var itemTag in itemTags)
            {
                await _itemRepository.AddItemTagAsync(itemTag);
            }

            // Insert ItemStats
            foreach (var itemStat in itemStats)
            {
                await _itemRepository.AddItemStatsAsync(itemStat);
            }

            return "Item synchronization completed successfully.";
        }
    }
}
