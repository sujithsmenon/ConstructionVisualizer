using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Shared
{
    public class ProjectImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public ImageCategory Category { get; set; }
        public ImageLayerType LayerType { get; set; }
        public int ZIndex { get; set; }
        public string Metadata { get; set; } // JSON for color, position, etc.

        public virtual Project Project { get; set; }
    }

    public enum ImageLayerType
    {
        Base,           // Main building structure
        Exterior,       // Walls, colors
        Roof,           // Roof shapes and materials
        Windows,        // Window styles
        Doors,          // Door designs
        Balcony,        // Balcony styles
        CarPorch,       // Car porch designs
        Garden,         // Landscaping
        Interior,       // Inside views
        Decoration      // Additional decorations
    }
}
