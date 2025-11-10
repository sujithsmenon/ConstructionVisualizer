using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Shared
{
    public class ProjectAccess
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
        public DateTime GrantedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }

        public virtual Project Project { get; set; }
    }
}
