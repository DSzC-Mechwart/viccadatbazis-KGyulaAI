using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViccController : ControllerBase
    {
        //Adatbázis kapcsolat
        private readonly ViccDbContext _context;

        public ViccController(ViccDbContext context)
        {
            _context = context;
        }

        //Viccek listázása
        [HttpGet]
        /*public ActionResult<List<Vicc>> GetViccek()
        {
            return _context.Viccek.Where(x => x.Aktiv == true).ToList();
        }*/
        public async Task<ActionResult<List<Vicc>>> GetViccek()
        {
            return await _context.Viccek.Where(x => x.Aktiv == true).ToListAsync();
        }

        //Egy vicc lekérdezése
        [HttpGet("{id}")]
        public async Task<ActionResult<Vicc>> GetVicc(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            return vicc == null ? NotFound() : vicc;
        }

        //Új vicc feltöltése
        [HttpPost]
        public async Task<ActionResult<Vicc>> PostVicc(Vicc ujVicc)
        {
            _context.Viccek.Add(ujVicc);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVicc", new {id = ujVicc.Id}, ujVicc);
            // Lekéri az adatbázisból az új viccet és azzal tér vissza
        }

        //Vicc módosítása
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVicc(int id, Vicc modositottVicc)
        {
            if (id != modositottVicc.Id)
            {
                return BadRequest();
            }

            _context.Entry(modositottVicc).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Vicc törlése
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVicc(int id)
        {
            var torlendoVicc = await _context.Viccek.FindAsync(id);
            if (torlendoVicc == null)
            {
                return NotFound();
            }
            if (torlendoVicc.Aktiv)
            {
                torlendoVicc.Aktiv = false;
            }
            else
            {
                _context.Viccek.Remove(torlendoVicc);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Vicc lájk
        [Route("{id}/like")] //https://localhost/api/Vicc/1/like
        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> Lajkolas(int id)
        {
            var vicc = _context.Viccek.Find(id);
            if (vicc == null)
            {
                return NotFound();
            }
            vicc.Tetszik++;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(vicc.Tetszik);
        }

        //Vicc dislájk
    }
}
