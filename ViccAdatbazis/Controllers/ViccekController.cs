using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ViccAdatbazis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViccekController : ControllerBase
    {
        private readonly ViccDbContext _context;

        public ViccekController(ViccDbContext context)
        {
            _context = context;
        }

        // GET: api/viccek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vicc>>> GetViccek(int page = 1)
        {
            int pageSize = 10;
            return await _context.Viccek
                .Where(v => v.Aktiv)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // POST: api/viccek
        [HttpPost]
        public async Task<ActionResult<Vicc>> PostVicc(Vicc vicc)
        {
            _context.Viccek.Add(vicc);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetViccek), new { id = vicc.Id }, vicc);
        }

        // PUT: api/viccek/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVicc(int id, Vicc vicc)
        {
            if (id != vicc.Id)
            {
                return BadRequest();
            }

            _context.Entry(vicc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViccExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/viccek/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVicc(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }

            vicc.Aktiv = false; // Archiválás
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/viccek/{id}/ertekeles
        [HttpPost("{id}/ertekeles")]
        public async Task<IActionResult> Ertekeles(int id, [FromBody] bool tetszik)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }

            if (tetszik)
            {
                vicc.Tetszik++;
            }
            else
            {
                vicc.NemTetszik++;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ViccExists(int id)
        {
            return _context.Viccek.Any(e => e.Id == id);
        }
    }
}
