using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace event_client_app.Controllers
{
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController(DBAPPContext dbContext)
        {
        }

        [HttpPut("users/{userId}/image")]
        public async Task<IActionResult> PutUserImage(int userId, IFormFile file)
        {
            // problem with upload type gif
            
            // var image = Request;
            // Console.WriteLine(image.Body);
            // Console.WriteLine(image.ContentType);

            // Unauthorized()
            // find userId is right  === not found 
            // check the userid is login user ==== Status(403)
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please put a image");
            }

            string[] typeContext = new[] {"image/gif", "image/jpeg", "image/jpg", "image/png"};

            if (!typeContext.Contains(file.ContentType))
            {
                return BadRequest("Please Check your image type is png, jpeg, jpg or gif");
            }


            string imageName = $"{userId}_{file.FileName}";
            // delete the old photo

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Storage_Image", imageName);


            // byte[] imageB = null;


            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);

            }

            // update the user table file name 
            // update database

            return Ok();
        }
    }
}