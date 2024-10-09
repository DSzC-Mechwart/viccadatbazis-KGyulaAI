using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ViccDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ViccDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Vicc> Viccek { get; set; }

        public async Task OnGetAsync()
        {
            Viccek = await _context.Viccek
                .Where(v => v.Aktiv)
                .ToListAsync();
        }
    }
}
