

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace event_client_app.Controllers
{
    [ApiController]
    public class ImageController : Controller
    {
        public ImageController(DBAPPContext dbContext)
        {
        }

        [HttpPut("users/{userId}/image")]
        public IActionResult PutUserImage(int userId)
        {
            var image = Request;
            Console.WriteLine(image.Body);
            Console.WriteLine(image.ContentType);
            // using ()
            // {
            //     
            // }

            return Ok();
        }
    }
}