using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ProniaApplication.Areas.ViewModels;
using ProniaApplication.Areas.ViewModels.Categories;
using ProniaApplication.DAL;
using ProniaApplication.Models;

namespace ProniaApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDBContext _context;

        public CategoryController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categoryVMs = await _context.Categories.Where(c => !c.IsDeleted).Include(c => c.Products).Select(c => new GetCategoryAdminVM
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.Products.Count
            }).ToListAsync();

            return View(categoryVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Categories.AnyAsync(c => c.Name.Trim() == categoryVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", $"{categoryVM.Name} is already exist");
                return View();
            }

            //categoryVM.CreatedAt = DateTime.Now;
            Category category = new() 
            { 
                Name = categoryVM.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            UpdateCategoryVM categoryVM = new UpdateCategoryVM
            { 
                Name = category.Name
            };

            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCategoryVM categoryVM)
        {
           

            if (id is null || id <= 0) return BadRequest();

            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (categoryVM is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            //existed.Name = category.Name;

            bool result = await _context.Categories.AnyAsync(c => c.Name.Trim() == categoryVM.Name.Trim() && c.Id != id);

            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), $"{categoryVM.Name} is already exist");
                return View();
            }

            //if (existed.Name == category.Name) return RedirectToAction(nameof(Index));

            existed.Name = categoryVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Archive(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            if (category.IsDeleted == false)
            {
                category.IsDeleted = true;
                
            }
            else
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }
    }
}
