using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.Areas.ViewModels.Sizes;
using ProniaApplication.DAL;
using ProniaApplication.Migrations;
using ProniaApplication.Models;


namespace ProniaApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly AppDBContext _context;

        public SizeController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Size> sizes = await _context.Sizes.ToListAsync();
            return View(sizes);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSizeVM sizeVM)
        {
            if(!ModelState.IsValid)
            {
                return View(sizeVM);
            }

            bool result = await _context.Sizes.AnyAsync(s=>s.SizeName.Trim() == sizeVM.SizeName.Trim());

            if(result)
            {
                ModelState.AddModelError(nameof(sizeVM.SizeName), $"{sizeVM.SizeName} is already exist");
                return View();
            }

            Models.Size size = new Size() 
            { 
                SizeName = sizeVM.SizeName
            };

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Size size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (size is null) return NotFound();

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Size size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (size is null) return NotFound();

            UpdateSizeVM sizeVM = new UpdateSizeVM()
            {
                SizeName = size.SizeName
            };
            return View(sizeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSizeVM sizeVM)
        {
            if(!ModelState.IsValid)
            {
                return View(sizeVM);
            }

            Size existed = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();

            existed.SizeName = sizeVM.SizeName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
