using Microsoft.AspNetCore.Mvc;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IItemRecommendationService _itemRecommendationService;

        public ItemController(IItemService itemService, IItemRecommendationService itemRecommendationService)
        {
            _itemService = itemService;
            _itemRecommendationService = itemRecommendationService;
        }

        [HttpGet]
        public async Task<IEnumerable<Item>> Get()
        {
            return await _itemService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Item item)
        {
            await _itemService.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            await _itemService.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _itemService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("recommended/{campeaoNome}")]
        public async Task<IEnumerable<Item>> GetRecommendedItems([FromRoute] string campeaoNome)
        {
            return await _itemRecommendationService.GetRecommendedItemsAsync(campeaoNome);
        }
    }
}
