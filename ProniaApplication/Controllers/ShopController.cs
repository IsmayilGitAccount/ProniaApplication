using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.DAL;
using ProniaApplication.Models;
using ProniaApplication.ViewModels;

namespace ProniaApplication.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDBContext _context;

        public ShopController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            Product? product = await _context.Products
                .Include(p=>p.productsImages
                .OrderByDescending(pi=>pi.IsPrimary))
                .Include(p=>p.category)
                .Include(p => p.ProductColors)
                .ThenInclude(pc=>pc.Color)
                .Include(p=>p.ProductSizes)
                .ThenInclude(pc=>pc.Size)
                .Include(p=>p.ProductTags)
                .ThenInclude(pt=>pt.Tag)
                
                .FirstOrDefaultAsync(p => p.Id == id);

            if (id == null) return NotFound();

            DetailsVM detailsVM = new DetailsVM
            {
                Product = product,
                RelatedProducts =await _context.Products
                .Where(p=>p.CategoryId==product.CategoryId && p.Id!=id)
                .Include(p=>p.productsImages.Where(pi=>pi.IsPrimary!=null))
                .Take(8)
                .ToListAsync()
            };
            return View(detailsVM);
        }
    }
}
