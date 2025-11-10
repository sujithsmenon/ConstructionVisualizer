using ConstructionVisualizer.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace ConstructionVisualizer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisualizationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        [HttpPost("customize")]
        public async Task<IActionResult> SaveCustomization([FromBody] CustomizationDto customization)
        {
            var userCustomization = new UserCustomization
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous",
                ProjectId = customization.ProjectId,
                CustomizationData = JsonSerializer.Serialize(customization.Selections),
                PreviewImage = customization.PreviewImage
            };

            _context.UserCustomizations.Add(userCustomization);
            await _context.SaveChangesAsync();

            return Ok(new { CustomizationId = userCustomization.Id });
        }

        [HttpGet("{projectId}/layers")]
        public async Task<IActionResult> GetProjectLayers(Guid projectId)
        {
            var layers = await _context.ProjectImages
                .Where(pi => pi.ProjectId == projectId)
                .OrderBy(pi => pi.ZIndex)
                .GroupBy(pi => pi.LayerType)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            return Ok(layers);
        }
    }
}
