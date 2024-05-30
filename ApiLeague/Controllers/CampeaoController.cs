using Microsoft.AspNetCore.Mvc;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampeaoController : ControllerBase
    {
        private readonly ICampeaoService _campeaoService;

        public CampeaoController(ICampeaoService campeaoService)
        {
            _campeaoService = campeaoService;
        }

        [HttpGet]
        public async Task<IEnumerable<Campeao>> Get()
        {
            return await _campeaoService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Campeao>> Get(int id)
        {
            var campeao = await _campeaoService.GetByIdAsync(id);
            if (campeao == null)
            {
                return NotFound();
            }
            return campeao;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Campeao campeao)
        {
            await _campeaoService.AddAsync(campeao);
            return CreatedAtAction(nameof(Get), new { id = campeao.Id }, campeao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Campeao campeao)
        {
            if (id != campeao.Id)
            {
                return BadRequest();
            }
            await _campeaoService.UpdateAsync(campeao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var campeao = await _campeaoService.GetByIdAsync(id);
            if (campeao == null)
            {
                return NotFound();
            }
            await _campeaoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
