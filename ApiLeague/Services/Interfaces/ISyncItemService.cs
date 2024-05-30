using System.Threading.Tasks;

namespace ApiLeague.Services.Interfaces
{
    public interface ISyncItemService
    {
        Task<string> SynchronizeItemsAsync();
    }
}
