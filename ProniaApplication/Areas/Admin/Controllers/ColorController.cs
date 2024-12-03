using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.Areas.ViewModels.Colors;
using ProniaApplication.DAL;
using ProniaApplication.Models;
using System.Collections.Generic;


namespace ProniaApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDBContext _context;
        

        public ColorController(AppDBContext context)
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Colors.ToListAsync();
            return View(colors);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateColorVM colorVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Colors.AnyAsync(c => c.Name.Trim() == colorVM.Name.Trim());

            if (result)
            {
                ModelState.AddModelError(nameof(colorVM.Name), $"{colorVM.Name} is already exist");
                return View();
            }

            Models.Color color = new()
            {
                Name = colorVM.Name
            };

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (color is null) return NotFound();

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Color color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (color is null) return NotFound();

            UpdateColorVM colorVM = new UpdateColorVM() 
            { 
                Name = color.Name
            };

            return View(colorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateColorVM colorVM)
        {
            if(!ModelState.IsValid)
            {
                return View(colorVM);
            }

            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();

            existed.Name = colorVM.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
