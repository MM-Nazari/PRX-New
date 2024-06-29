using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models; // Assuming your User and UserDocument models are in this namespace
using PRX.Models.User;
using PRX.Utils;
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
        private readonly ILogger<UserDocumentController> _logger;
        public UserDocumentController(PRXDbContext context, ILogger<UserDocumentController> logger) 
        { 
            _context = context;
            _logger = logger;
        }

        [HttpPost("UserDocuments")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UploadDocument([FromForm] UserDocumentDto documentDto)
        {
            try
            {
                // Retrieve the request based on the RequestId
                var request = await _context.Requests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == documentDto.RequestId);
                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                var user = request.User;
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                if (user.Id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
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
                        fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.pdf";
                        break;
                    case ".png":
                        fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.png";
                        break;
                    case ".jpeg":
                    case ".jpg":
                        fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.jpeg";
                        break;
                    case ".zip": // Handle .zip files
                        fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.zip";
                        break;
                    default:
                        return BadRequest(new { message = ResponseMessages.UserFileFormatConfliction });
                }

                // Construct the full file path
                var filePath = Path.Combine(userFolder, fileName);

                // Save the file to the user's folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentDto.File.CopyToAsync(stream);
                }

                // Save the file path to the database
                var userDocument = new UserDocument
                {
                    RequestId = documentDto.RequestId,
                    DocumentType = documentDto.DocumentType,
                    FilePath = filePath,
                    IsDeleted = false
                };

                _context.UserDocuments.Add(userDocument);
                await _context.SaveChangesAsync();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        //[HttpPost("UserDocuments")]
        //[Authorize(Roles = "User")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UploadDocument([FromForm] UserDocumentDto documentDto)
        //{

        //    try
        //    {
        //        var user = await _context.Users.FindAsync(documentDto.RequestId);
        //        if (user == null)
        //        {
        //            return NotFound(new { message = ResponseMessages.UserNotFound });
        //        }

        //        var tokenUserId = int.Parse(User.FindFirst("id")?.Value);



        //        if (documentDto.RequestId != tokenUserId)
        //        {
        //            return Unauthorized(new { message = ResponseMessages.Unauthorized });
        //        }

        //        // Create a folder with the user's phone number if it doesn't exist
        //        var userFolder = Path.Combine("C:\\PRX Documents", user.PhoneNumber);
        //        if (!Directory.Exists(userFolder))
        //        {
        //            Directory.CreateDirectory(userFolder);
        //        }

        //        // Get the file extension from the uploaded file
        //        var fileExtension = Path.GetExtension(documentDto.File.FileName).ToLower();

        //        // Determine the file name and extension based on the document type
        //        string fileName;
        //        switch (fileExtension)
        //        {
        //            case ".pdf":
        //                fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.pdf";
        //                break;
        //            case ".png":
        //                fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.png";
        //                break;
        //            case ".jpeg":
        //            case ".jpg":
        //                fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.jpeg";
        //                break;
        //            case ".zip": // Handle .zip files
        //                fileName = $"{documentDto.RequestId}-{documentDto.DocumentType}.zip";
        //                break;
        //            default:
        //                return BadRequest(new { message = ResponseMessages.UserFileFormatConfliction });
        //        }

        //        // Construct the full file path
        //        var filePath = Path.Combine(userFolder, fileName);

        //        //// Generate a unique file name based on user ID and document type
        //        //var fileName = $"{userId}-{documentDto.DocumentType}.png";
        //        //var filePath = Path.Combine(userFolder, fileName);

        //        // Save the file to the user's folder
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await documentDto.File.CopyToAsync(stream);
        //        }

        //        // Save the file path to the database
        //        var userDocument = new UserDocument
        //        {
        //            RequestId = documentDto.RequestId, // Correct casing for UserID
        //            DocumentType = documentDto.DocumentType,
        //            FilePath = filePath,
        //            IsDeleted = false
        //        };

        //        _context.UserDocuments.Add(userDocument);
        //        await _context.SaveChangesAsync();

        //        return Ok(new { message = ResponseMessages.OK });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
        //    }

        //}


        [HttpPut("UserDocuments/{requestId}/{documentType}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDocument(int requestId, string documentType, [FromForm] UserDocumentDto documentDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the request based on the requestId
                var request = await _context.Requests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                var user = request.User;
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                // Ensure the request is made by the authenticated user
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                if (user.Id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                // Find the existing document by requestId and documentType
                var existingDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.RequestId == requestId && d.DocumentType == documentType);
                if (existingDocument == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDocNotFound });
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
                        fileName = $"{requestId}-{documentType}.pdf";
                        break;
                    case ".png":
                        fileName = $"{requestId}-{documentType}.png";
                        break;
                    case ".jpeg":
                    case ".jpg":
                        fileName = $"{requestId}-{documentType}.jpeg";
                        break;
                    case ".zip": // Handle .zip files
                        fileName = $"{requestId}-{documentType}.zip";
                        break;
                    default:
                        return BadRequest(new { message = ResponseMessages.UserFileFormatConfliction });
                }

                // Construct the full file path
                var filePath = Path.Combine(userFolder, fileName);

                // Save the file to the user's folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentDto.File.CopyToAsync(stream);
                }

                // Update the existing document
                existingDocument.FilePath = filePath;
                existingDocument.DocumentType = documentDto.DocumentType;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        //[HttpPut("UserDocuments/{requestId}/{documentType}")]
        //[Authorize(Roles = "User")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateDocument(int requestId, string documentType, [FromForm] UserDocumentDto documentDto)
        //{
        //    try
        //    {

        //        if (requestId <= 0)
        //        {
        //            return BadRequest(new { message = ResponseMessages.InvalidId });
        //        }

        //        // Check if the user exists
        //        var user = await _context.Users.FindAsync(requestId);
        //        if (user == null)
        //        {
        //            return NotFound(new { message = ResponseMessages.UserNotFound });
        //        }

        //        // Ensure the request is made by the authenticated user
        //        var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
        //        if (requestId != tokenUserId)
        //        {
        //            return Unauthorized(new { message = ResponseMessages.Unauthorized });
        //        }

        //        // Find the existing document by userId and documentType
        //        var existingDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.RequestId == requestId && d.DocumentType == documentType);
        //        if (existingDocument == null)
        //        {
        //            return NotFound(new { message = ResponseMessages.UserDocNotFound });
        //        }

        //        // Create a folder with the user's phone number if it doesn't exist
        //        var userFolder = Path.Combine("C:\\PRX Documents", user.PhoneNumber);
        //        if (!Directory.Exists(userFolder))
        //        {
        //            Directory.CreateDirectory(userFolder);
        //        }

        //        // Get the file extension from the uploaded file
        //        var fileExtension = Path.GetExtension(documentDto.File.FileName).ToLower();

        //        // Determine the file name and extension based on the document type
        //        string fileName;
        //        switch (fileExtension)
        //        {
        //            case ".pdf":
        //                fileName = $"{requestId}-{documentType}.pdf";
        //                break;
        //            case ".png":
        //                fileName = $"{requestId}-{documentType}.png";
        //                break;
        //            case ".jpeg":
        //            case ".jpg":
        //                fileName = $"{requestId}-{documentType}.jpeg";
        //                break;
        //            case ".zip": // Handle .zip files
        //                fileName = $"{requestId}-{documentType}.zip";
        //                break;
        //            default:
        //                return BadRequest(new { message = ResponseMessages.UserFileFormatConfliction });
        //        }

        //        // Construct the full file path
        //        var filePath = Path.Combine(userFolder, fileName);

        //        // Save the file to the user's folder
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await documentDto.File.CopyToAsync(stream);
        //        }

        //        // Update the existing document
        //        existingDocument.FilePath = filePath;
        //        existingDocument.DocumentType = documentDto.DocumentType;

        //        // Save changes to the database
        //        await _context.SaveChangesAsync();

        //        return Ok(new { message = ResponseMessages.OK });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
        //    }
        //}



        //[HttpPut("UserDocuments/{userId}/{documentType}")]
        //[Authorize(Roles = "User")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateDocument(int userId, string documentType, [FromForm] UserDocumentDto updatedDocumentDto)
        //{
        //    var user = await _context.Users.FindAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
        //    if (userId != tokenUserId)
        //    {
        //        return Forbid(); // Or return 403 Forbidden
        //    }

        //    var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == documentType);
        //    if (userDocument == null)
        //    {
        //        return NotFound();
        //    }

        //    // Create a folder with the user's phone number if it doesn't exist
        //    var userFolder = Path.Combine("C:\\PRX Documents", user.PhoneNumber);
        //    if (!Directory.Exists(userFolder))
        //    {
        //        Directory.CreateDirectory(userFolder);
        //    }

        //    // Get the file extension from the uploaded file
        //    var fileExtension = Path.GetExtension(updatedDocumentDto.File.FileName).ToLower();

        //    // Determine the file name and extension based on the document type
        //    string fileName;
        //    switch (fileExtension)
        //    {
        //        case ".pdf":
        //            fileName = $"{userId}/{documentType}.pdf";
        //            break;
        //        case ".png":
        //            fileName = $"{userId}/{documentType}.png";
        //            break;
        //        case ".jpeg":
        //        case ".jpg":
        //            fileName = $"{userId}/{documentType}.jpeg";
        //            break;
        //        default:
        //            return BadRequest("Unsupported file format.");
        //    }

        //    // Construct the full file path
        //    var filePath = Path.Combine(userFolder, fileName);

        //    // Save the file to the user's folder
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await updatedDocumentDto.File.CopyToAsync(stream);
        //    }

        //    // Update the file path in the database
        //    userDocument.FilePath = filePath;

        //    // Save changes to the database
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}


        [HttpGet("UserDocuments/{requestId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserDocuments(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDocuments = await _context.UserDocuments.Where(d => d.RequestId == requestId).ToListAsync();
                return Ok(userDocuments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }




        [HttpGet("UserDocuments/{requestId}/{documentType}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDocument(int requestId, string documentType)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the request based on the requestId
                var request = await _context.Requests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                var user = request.User;
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                //var user = await _context.Users.FindAsync(requestId);
                //if (user == null)
                //{
                //    return NotFound(new {message = ResponseMessages.UserNotFound});
                //}

                // Authorization check: Ensure user can only update their own documents
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                if (user.Id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.RequestId == requestId && d.DocumentType == documentType);
                if (userDocument == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDocNotFound });
                }

                // Ensure that the file path belongs to the respective user's folder
                if (!userDocument.FilePath.StartsWith($"C:\\PRX Documents\\{user.PhoneNumber}"))
                {
                    return Unauthorized(new { message = ResponseMessages.UserFilePathConfliction }); // Prevent accessing files of other users
                }

                // Retrieve the file from the specified file path
                var filePath = userDocument.FilePath;
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpDelete("UserDocuments/{requestId}/{documentType}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MarkDocumentAsDeleted(int requestId, string documentType)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the request based on the requestId
                var request = await _context.Requests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                var user = request.User;
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                //var user = await _context.Users.FindAsync(requestId);
                //if (user == null)
                //{
                //    return NotFound(new { message = ResponseMessages.UserNotFound });
                //}

                var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.RequestId == requestId && d.DocumentType == documentType);
                if (userDocument == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDocNotFound });
                }

                userDocument.IsDeleted = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        // Admin Controllers 

        // Admin endpoints
        [HttpGet("UserDocuments/Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDocumentsAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDocuments = await _context.UserDocuments.Where(d => d.RequestId == requestId).ToListAsync();
                return Ok(userDocuments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPut("UserDocuments/Admin/{requestId}/{documentType}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDocumentAdmin(int requestId, string documentType, [FromForm] UserDocumentDto updatedDocumentDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (updatedDocumentDto == null || updatedDocumentDto.File == null)
                {
                    return BadRequest(new { message = ResponseMessages.UserFileNotFound });
                }

                // Retrieve the request based on the requestId
                var request = await _context.Requests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                var user = request.User;
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                //var user = await _context.Users.FindAsync(requestId);
                //if (user == null)
                //{
                //    return NotFound(new { message = ResponseMessages.UserNotFound });
                //}

                var userDocument = await _context.UserDocuments.FirstOrDefaultAsync(d => d.RequestId == requestId && d.DocumentType == documentType);
                if (userDocument == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDocNotFound });
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
                        fileName = $"{requestId}-{documentType}.pdf";
                        break;
                    case ".png":
                        fileName = $"{requestId}-{documentType}.png";
                        break;
                    case ".jpeg":
                    case ".jpg":
                        fileName = $"{requestId}-{documentType}.jpeg";
                        break;
                    default:
                        return BadRequest(new { message = ResponseMessages.UserFileFormatConfliction });
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

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("Admin/UserDocuments")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDocuments()
        {
            try
            {
                var userDocuments = await _context.UserDocuments.ToListAsync();

                if (userDocuments == null || !userDocuments.Any())
                {
                    return NotFound(new { message = ResponseMessages.UserDocNotFound });
                }

                // Map UserDocument entities to DTOs
                var documentDtos = userDocuments.Select(document => new UserDocumentDto
                {
                    RequestId = document.RequestId,
                    DocumentType = document.DocumentType,
                    //FilePath = document.FilePath,
                    IsDeleted = document.IsDeleted
                }).ToList();

                return Ok(documentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


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
