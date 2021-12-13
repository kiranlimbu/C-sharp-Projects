namespace bugspotAPI.Dtos
{
    public class BugEditDto
    {
        public string title { get; set; }
        public int typeId { get; set; }
        public int statusId { get; set; }
        public int priorityId { get; set; }
        public int severityId { get; set; }
        public string reporterId { get; set; }
        public string developerId { get; set; }

        // need comment, attachment,
    }
}