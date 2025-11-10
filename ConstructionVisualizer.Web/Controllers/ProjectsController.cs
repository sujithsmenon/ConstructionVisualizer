using ConstructionVisualizer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ConstructionVisualizer.Shared.Services; // Ensure this matches the namespace where IBlobStorageService is defined.

namespace ConstructionVisualizer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorage;

        public ProjectsController(ApplicationDbContext context, IBlobStorageService blobStorage)
        {
            _context = context;
            _blobStorage = blobStorage;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Type = projectDto.Type,
                Description = projectDto.Description,
                AdminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                IsPublic = projectDto.IsPublic,
                SharePassword = BCrypt.Net.BCrypt.HashPassword(projectDto.SharePassword)
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        [HttpPost("{projectId}/upload-image")]
        public async Task<IActionResult> UploadImage(Guid projectId, [FromForm] ImageUploadDto uploadDto)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null || project.AdminId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return Forbid();

            var imageUrl = await _blobStorage.UploadImageAsync(uploadDto.Image, "projects");
            var thumbnailUrl = await _blobStorage.GenerateThumbnailAsync(imageUrl, 300, 300);

            var projectImage = new ProjectImage
            {
                ProjectId = projectId,
                ImageUrl = imageUrl,
                ThumbnailUrl = thumbnailUrl,
                Category = uploadDto.Category,
                LayerType = uploadDto.LayerType,
                ZIndex = uploadDto.ZIndex,
                Metadata = uploadDto.Metadata
            };

            _context.ProjectImages.Add(projectImage);
            await _context.SaveChangesAsync();

            return Ok(projectImage);
        }

        [HttpGet("shared/{projectId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSharedProject(Guid projectId, [FromQuery] string password)
        {
            var project = await _context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null || !project.IsPublic)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(password, project.SharePassword))
                return Unauthorized("Invalid password");

            return Ok(project);
        }
    }
}
