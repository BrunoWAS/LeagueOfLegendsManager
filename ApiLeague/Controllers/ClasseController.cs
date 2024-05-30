using Microsoft.AspNetCore.Mvc;
using ApiLeague.Services.Interfaces;

namespace ApiLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClasseController : ControllerBase
    {
        private readonly IClasseService _classeService;

        public ClasseController(IClasseService classeService)
        {
            _classeService = classeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Classe>> Get()
        {
            return await _classeService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Classe>> Get(int id)
        {
            var classe = await _classeService.GetByIdAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            return classe;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Classe classe)
        {
            await _classeService.AddAsync(classe);
            return CreatedAtAction(nameof(Get), new { id = classe.Id }, classe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Classe classe)
        {
            if (id != classe.Id)
            {
                return BadRequest();
            }
            await _classeService.UpdateAsync(classe);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var classe = await _classeService.GetByIdAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            await _classeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
