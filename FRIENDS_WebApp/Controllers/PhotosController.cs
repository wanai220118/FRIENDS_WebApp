using FRIENDS_WebApp.Data;
using FRIENDS_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FRIENDS_WebApp.Controllers
{
    [Authorize]
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.ToListAsync());
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Photo photo, IFormFile ImageFile)
        {
            Console.WriteLine("🔥 POST /Photos/Create was triggered");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Only image files (.jpg, .png, etc.) are allowed.");
                    return View(photo);
                }

                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                photo.ImagePath = "/uploads/" + fileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please select an image file.");
                return View(photo);
            }

            // Temporarily bypass all model validation
            photo.DateUploaded = DateTime.Now;

            _context.Add(photo);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Memory uploaded successfully!";
            TempData["ToastColor"] = "bg-success";
            return RedirectToAction(nameof(Index));
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Photo photo, IFormFile NewImageFile)
        {
            var photoToUpdate = await _context.Photos.FindAsync(id);
            if (photoToUpdate == null)
            {
                return NotFound();
            }

            // Update title and caption
            photoToUpdate.Title = photo.Title;
            photoToUpdate.Caption = photo.Caption;

            // If a new image is uploaded
            if (NewImageFile != null && NewImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                var ext = Path.GetExtension(NewImageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("NewImageFile", "Only image files (.jpg, .png, etc.) are allowed.");
                    return View(photoToUpdate);
                }

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var newFileName = Guid.NewGuid().ToString() + ext;
                var newFilePath = Path.Combine(uploadsPath, newFileName);

                // Delete old image
                if (!string.IsNullOrEmpty(photoToUpdate.ImagePath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photoToUpdate.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Update path
                photoToUpdate.ImagePath = "/uploads/" + newFileName;
            }

            photoToUpdate.DateUploaded = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Memory updated successfully!";
            TempData["ToastColor"] = "bg-primary";
            return RedirectToAction(nameof(Index));
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Memory deleted successfully!";
            TempData["ToastColor"] = "bg-danger";
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}
