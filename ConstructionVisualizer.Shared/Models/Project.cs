using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Shared
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public ProjectType Type { get; set; }
        public string Description { get; set; }
        public bool IsUnderConstruction { get; set; }
        public string AdminId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsPublic { get; set; }
        public string SharePassword { get; set; }

        // Navigation properties
        public virtual ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
        public virtual ICollection<ProjectAccess> SharedAccess { get; set; } = new List<ProjectAccess>();
    }
}
