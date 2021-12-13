namespace bugspotAPI.Dtos
{
    public class BugDto
    {
        public string title { get; set; }
        public int typeId { get; set; }
        public int statusId { get; set; }
        public int priorityId { get; set; }
        public int severityId { get; set; }
        public string reporterId { get; set; }
        public string developerId { get; set; }
        public string description { get; set; }
        public string stepsToProd { get; set; }
        public string actualRes { get; set; }
        public string expectedRes { get; set; }

        // need comment, attachment,
    }
}