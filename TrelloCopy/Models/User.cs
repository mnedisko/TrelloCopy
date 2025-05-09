namespace TrelloCopy.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ?UserName { get; set; }
        public string ?Email { get; set; }
        public string ? PasswordHash { get; set; }
        public DateTime ? CreatedAt { get; set; }
        public DateTime ? UpdatedAt { get; set; }
        public string  Authority { get; set; }
        public List<Project> Projects { get; set; }
        public List<ProjectUser> projectUsers { get; set; }

    }
}
 