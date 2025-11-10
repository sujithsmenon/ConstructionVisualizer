using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Shared
{
    public class UserCustomization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string CustomizationData { get; set; } // JSON of all selections
        public string PreviewImage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual Project Project { get; set; }
    }
}
