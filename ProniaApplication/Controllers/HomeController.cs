using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.DAL;
using ProniaApplication.Models;
using ProniaApplication.ViewModels;

namespace ProniaApplication.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDBContext _context;
        public HomeController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Slides = await _context.Slides
                .OrderBy(s => s.Order)
                .Take(3)
                .ToListAsync(),


                Products = await _context.Products
                .Take(8)
                .Include(p=>p.productsImages.Where(pi=>pi.IsPrimary!=null))
                .ToListAsync()
            };
            return View(homeVM);
        }

        
    }
}
