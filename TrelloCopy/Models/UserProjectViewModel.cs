namespace TrelloCopy.Models
{
    public class UserProjectViewModel
    {
        public string UserName { get; set; }
        public List<ProjectInfo> OwnedProject { get; set; }
        public List<ProjectInfo> AssignedProject { get; set; }
    }
    public class ProjectInfo
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserRole { get; set; }

    }

}
