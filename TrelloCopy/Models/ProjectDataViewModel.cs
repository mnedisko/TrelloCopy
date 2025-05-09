using System.ComponentModel.DataAnnotations;

namespace TrelloCopy.Models
{
    public class ProjectDataViewModel
    {
        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public string ProjectTitle { get; set; }
    }
}
