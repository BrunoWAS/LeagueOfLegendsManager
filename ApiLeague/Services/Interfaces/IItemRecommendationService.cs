namespace ApiLeague.Services.Interfaces
{
    public interface IItemRecommendationService
    {
        Task<IEnumerable<Item>> GetRecommendedItemsAsync(string campeaoNome);
    }
}
