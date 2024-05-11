using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models; // Assuming your User and UserDocument models are in this namespace
using PRX.Models.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PRX.Controllers.User
{
    [ApiController]
    [Route("api")]
    [ApiExplorerSettings(GroupName = "UserDocuments")]
    public class UserDocumentController : ControllerBase
    {
        private readonly PRXDbContext _context;
        public UserDocumentController(PRXDbContext context) 
        { 
            _context = context;
        }

        [HttpPost("UserDocuments")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadDocument([FromForm] UserDocumentDto documentDto)
        {
            var user = await _context.Users.FindAsync(documentDto.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
            if (documentDto.UserId != tokenUserId)
            {
                return Forbid(); // Or return 403 Forbidden
            }

            // Create a folder with the user's phone number if it doesn't exist
            var userFolder = Path.Combine("C:\\PRX Documents", user.PhoneNumber);
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            // Get the file extension from the uploaded file
            var fileExtension = Path.GetExtension(documentDto.File.FileName).ToLower();

            // Determine the file name and extension based on the document type
            string fileName;
            switch (fileExtension)
            {
                case ".pdf":
                    fileName = $"{documentDto.UserId}-{documentDto.DocumentType}.pdf";
                    break;
                case ".png":
                    fileName = $"{documentDto.UserId}-{documentDto.DocumentType}.png";
                    break;
                case ".jpeg":
                case ".jpg":
                    fileName = $"{documentDto.UserId}-{documentDto.DocumentType}.jpeg";
                    break;
                default:
                    return BadRequest("Unsupported file format.");
            }

            // Construct the full file path
            var filePath = Path.Combine(userFolder, fileName);

            //// Generate a unique file name based on user ID and document type
            //var fileName = $"{userId}-{documentDto.DocumentType}.png";
            //var filePath = Path.Combine(userFolder, fileName);

            // Save the file to the user's folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await documentDto.File.CopyToAsync(stream);
            }

            // Save the file path to the database
            var userDocument = new UserDocument
            {
                UserId = documentDto.UserId, // Correct casing for UserID
                DocumentType = documentDto.DocumentType,
                FilePath = filePath,
                IsDeleted = false
            };

            _context.UserDocuments.Add(userDocument);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("UserDocuments/{userId}/{documentType}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDocument(int userId, string documentType, [FromForm] UserDocumentDto updatedDocumentDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
            if (userId != tokenUserId)
            {
                return Forbid(); // Or return 403 Forbidden
            }

            var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == documentType);
            if (userDocument == null)
            {
                return NotFound();
            }

            // Create a folder with the user's phone number if it doesn't exist
            var userFolder = Path.Combine("C:\\PRX Documents", user.PhoneNumber);
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            // Get the file extension from the uploaded file
            var fileExtension = Path.GetExtension(updatedDocumentDto.File.FileName).ToLower();

            // Determine the file name and extension based on the document type
            string fileName;
            switch (fileExtension)
            {
                case ".pdf":
                    fileName = $"{userId}-{documentType}.pdf";
                    break;
                case ".png":
                    fileName = $"{userId}-{documentType}.png";
                    break;
                case ".jpeg":
                case ".jpg":
                    fileName = $"{userId}-{documentType}.jpeg";
                    break;
                default:
                    return BadRequest("Unsupported file format.");
            }

            // Construct the full file path
            var filePath = Path.Combine(userFolder, fileName);

            // Save the file to the user's folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updatedDocumentDto.File.CopyToAsync(stream);
            }

            // Update the file path in the database
            userDocument.FilePath = filePath;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok();
        }






        [HttpGet("UserDocuments/{userId}/{documentType}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDocument(int userId, string documentType)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Authorization check: Ensure user can only update their own documents
            var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
            if (userId != tokenUserId)
            {
                return Forbid(); // Or return 403 Forbidden
            }

            var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == documentType);
            if (userDocument == null)
            {
                return NotFound();
            }

            // Ensure that the file path belongs to the respective user's folder
            if (!userDocument.FilePath.StartsWith($"C:\\PRX Documents\\{user.PhoneNumber}"))
            {
                return Unauthorized(); // Prevent accessing files of other users
            }

            // Retrieve the file from the specified file path
            var filePath = userDocument.FilePath;
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
        }


        [HttpDelete("UserDocuments/{userId}/{documentType}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MarkDocumentAsDeleted(int userId, string documentType)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == documentType);
            if (userDocument == null)
            {
                return NotFound();
            }

            userDocument.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok();
        }


        // Admin Controllers 

        // Admin endpoints
        [HttpGet("UserDocuments/Admin/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDocuments(int userId)
        {
            var userDocuments = await _context.UserDocuments.Where(d => d.UserId == userId).ToListAsync();
            return Ok(userDocuments);
        }

        [HttpPut("UserDocuments/Admin{userId}/{documentType}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDocumentAdmin(int userId, string documentType, [FromForm] UserDocumentDto updatedDocumentDto)
        {
            if (updatedDocumentDto == null || updatedDocumentDto.File == null)
            {
                return BadRequest("File is required.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == documentType);
            if (userDocument == null)
            {
                return NotFound();
            }

            // Create a folder with the user's phone number if it doesn't exist
            var userFolder = Path.Combine("C:\\PRX Documents", user.PhoneNumber);
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            // Get the file extension from the uploaded file
            var fileExtension = Path.GetExtension(updatedDocumentDto.File.FileName).ToLower();

            // Determine the file name and extension based on the document type
            string fileName;
            switch (fileExtension)
            {
                case ".pdf":
                    fileName = $"{userId}-{documentType}.pdf";
                    break;
                case ".png":
                    fileName = $"{userId}-{documentType}.png";
                    break;
                case ".jpeg":
                case ".jpg":
                    fileName = $"{userId}-{documentType}.jpeg";
                    break;
                default:
                    return BadRequest("Unsupported file format.");
            }

            // Construct the full file path
            var filePath = Path.Combine(userFolder, fileName);

            // Save the file to the user's folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updatedDocumentDto.File.CopyToAsync(stream);
            }

            // Update the file path in the database
            userDocument.FilePath = filePath;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Admin/UserDocuments")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDocuments()
        {
            var userDocuments = await _context.UserDocuments.ToListAsync();

            if (userDocuments == null || !userDocuments.Any())
            {
                return NotFound();
            }

            // Map UserDocument entities to DTOs
            var documentDtos = userDocuments.Select(document => new UserDocumentDto
            {
                UserId = document.UserId,
                DocumentType = document.DocumentType,
                //FilePath = document.FilePath,
                IsDeleted = document.IsDeleted
            }).ToList();

            return Ok(documentDtos);
        }


        //[HttpGet("Admin/UserDocuments")]
        //[Authorize(Roles = "Admin")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetAllDocuments()
        //{
        //    var userDocuments = await _context.UserDocuments.ToListAsync();

        //    if (userDocuments == null || !userDocuments.Any())
        //    {
        //        return NotFound();
        //    }

        //    // Map UserDocument entities to DTOs
        //    var documentDtos = userDocuments.Select(document => new UserDocumentDto
        //    {
        //        UserId = document.UserId,
        //        DocumentType = document.DocumentType,
        //        FilePath = document.FilePath, // Include FilePath instead of File
        //        IsDeleted = document.IsDeleted
        //    }).ToList();

        //    return Ok(documentDtos);
        //}






    }
}
