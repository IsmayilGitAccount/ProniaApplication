using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using ProniaApplication.Areas.ViewModels;
using ProniaApplication.DAL;
using ProniaApplication.Models;
using ProniaApplication.Utilities.Enums;
using ProniaApplication.Utilities.Extension;

namespace ProniaApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {


            if (!slideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is not correct");
                return View();
            }
            if (!slideVM.Photo.ValidateSize(Utilities.Enums.FileSize.MB, 2))
            {
                ModelState.AddModelError("Photo", "File size must be less than 2 MB");
                return View();
            }



            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            //await _context.Slides.AddAsync(slide);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));


            Slide slide = new Slide
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Order = slideVM.Order,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };
            slide.Image = await slideVM.Photo.CreatFileAsync(_env.WebRootPath, "assets", "images", "website-images");
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();
            UpdateSlideVM slideVM = new UpdateSlideVM
            {  
            Title = slide.Title,
            SubTitle = slide.SubTitle,
            Description = slide.Description,
            Order = slide.Order,
            Image = slide.Image
            };

            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            

            //slideVM.Image = slide.Image;

            if (!ModelState.IsValid) 
            {
                return View(slideVM);
            }

            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();

            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM), "Type is incorrect");
                    return View(slideVM);
                }
                if (!slideVM.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM), "Type size incorrect");
                    return View(slideVM);
                }

                string fileName = await slideVM.Photo.CreatFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image = fileName;
            }

            existed.Title = slideVM.Title;
            existed.Description = slideVM.Description;
            existed.SubTitle = slideVM.SubTitle;
            existed.Order = slideVM.Order;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s=>s.Id == id);
            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
