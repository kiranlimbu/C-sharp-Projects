using System;

namespace bugspotAPI.Dtos
{
    public class ProjectDto
    {
        public int? companyId { get; set; }
        public string projName { get; set; }
        public string projDescription { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset? endDate { get; set; }
        public bool archived { get; set; }
    }
}