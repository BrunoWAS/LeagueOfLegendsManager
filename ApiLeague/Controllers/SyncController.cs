using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;
        private readonly ISyncItemService _syncItemService;

        public SyncController(ISyncService syncService, ISyncItemService syncItemService)
        {
            _syncService = syncService;
            _syncItemService = syncItemService;
        }

        [HttpPost]
        [Route("synchronize")]
        public async Task<IActionResult> Synchronize()
        {
            var result = await _syncService.SynchronizeDataAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("synchronizeItems")]
        public async Task<IActionResult> SynchronizeItems()
        {
            var result = await _syncItemService.SynchronizeItemsAsync();
            return Ok(result);
        }
    }
}
