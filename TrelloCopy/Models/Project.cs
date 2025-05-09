namespace TrelloCopy.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ?UpdatedAt { get; set; }
        public string ?ProjectTitle { get; set; }
        public List<ProjectUser> projectUsers { get; set; }
        public List<Tasks> Tasks { get; set; }
    }
}
