using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.Areas.ViewModels;
using ProniaApplication.Areas.ViewModels.Tags;
using ProniaApplication.DAL;
using ProniaApplication.Models;

namespace ProniaApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDBContext _context;

        public TagController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();
            return View(tags);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateTagVM tagVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Tags.AnyAsync(t=>t.Name.Trim()== tagVM.Name.Trim());

            if(result==true)
            {
                ModelState.AddModelError("Name", $"{tagVM.Name} is already exist");
                return View();
            }

            Tag tag = new()
            {
                Name = tagVM.Name
               
            };
           

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag is null) return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(s => s.Id == id);
            if (tag is null) return NotFound();

            UpdateTagVM tagVM = new UpdateTagVM
            {
                Name = tag.Name
            };

            return View(tagVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int? id, UpdateTagVM tagVM)
        {
            if (!ModelState.IsValid)
            {
                return View(tagVM);
            }

            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();

            existed.Name = tagVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
