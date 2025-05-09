using System.Data;

namespace TrelloCopy.Models
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string RoleName { get; set; }
        
        public DateTime AssignedAt { get; set; }
    }
}
