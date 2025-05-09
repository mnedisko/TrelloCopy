namespace TrelloCopy.Models
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public UsersInfo UsersInfo { get; set; }
        public List<TaskInfo> TaskInfo { get; set; }
    }
    public class UsersInfo
    {
       
        public List<User> AvailableUsers { get; set; }
        public List<string> Roles { get; set; }
        public string CurrentUserRole { get; set; }
    }
    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskContent { get; set; }
        public string AssignedToUserName { get; set; }
        public string TaskStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
