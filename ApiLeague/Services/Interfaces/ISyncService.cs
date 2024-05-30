using System.Threading.Tasks;

namespace ApiLeague.Services.Interfaces
{
    public interface ISyncService
    {
        Task<string> SynchronizeDataAsync();
    }
}
