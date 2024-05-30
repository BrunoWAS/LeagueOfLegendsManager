using ApiLeague.Data.Repository.Interfaces;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Services
{
    public class ItemRecommendationService : IItemRecommendationService
    {
        private readonly ICampeaoRepository _campeaoRepository;
        private readonly IItemRepository _itemRepository;

        private readonly Dictionary<string, List<string>> _classTagMapping = new Dictionary<string, List<string>>
        {
            { "Marksman", new List<string> { "AttackSpeed", "CriticalStrike", "LifeSteal", "OnHit" } },
            { "Mage", new List<string> { "SpellDamage", "Mana", "SpellPenetration" } },
            { "Fighter", new List<string> { "Damage", "Health", "Tenacity" } },
            { "Tank", new List<string> { "Health", "Armor", "MagicResist", "HealthRegen", "CrowdControlReduction" } },
            { "Assassin", new List<string> { "Damage", "ArmorPenetration", "CooldownReduction", "AbilityHaste", "Stealth", "NonbootsMovement", "Lethality" } },
            { "Support", new List<string> { "GoldPer", "HealthRegen", "ManaRegen", "CooldownReduction", "AbilityHaste" } }
        };

        public ItemRecommendationService(ICampeaoRepository campeaoRepository, IItemRepository itemRepository)
        {
            _campeaoRepository = campeaoRepository;
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> GetRecommendedItemsAsync(string campeaoNome)
        {
            var campeao = (await _campeaoRepository.GetAllAsync()).FirstOrDefault(c => c.Nome == campeaoNome);
            if (campeao == null)
            {
                return Enumerable.Empty<Item>();
            }

            var recommendedTags = new HashSet<string>();

            foreach (var classe in campeao.Classes)
            {
                if (_classTagMapping.ContainsKey(classe.Nome))
                {
                    foreach (var tag in _classTagMapping[classe.Nome])
                    {
                        recommendedTags.Add(tag);
                    }
                }
            }

            var allItems = await _itemRepository.GetAllAsync();
            var recommendedItems = new List<Item>();

            foreach (var item in allItems)
            {
                var itemTags = item.Tags.Select(t => t.Nome).ToList();
                if (itemTags.Any(tag => recommendedTags.Contains(tag)))
                {
                    recommendedItems.Add(item);
                }
            }

            return recommendedItems;
        }
    }
}
