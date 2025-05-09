using System.ComponentModel.DataAnnotations;

namespace TrelloCopy.Models
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
        public string TaskDescription { get; set; }
        public string TaskStatus { get; set; } = "New";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ReviewedByUserId { get; set; }
        public User ReviewedByUser { get; set; }
        public string ReviewStatus { get; set; }
        public string TaskTitle { get; set; }
    }
}
