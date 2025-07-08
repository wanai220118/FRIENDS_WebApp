using Microsoft.AspNetCore.Mvc;

namespace FRIENDS_WebApp.Controllers
{
    public class TestUploadController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return Content("❌ No file received.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", ImageFile.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

            return Content("✅ File uploaded: " + ImageFile.FileName);
        }
    }
}
