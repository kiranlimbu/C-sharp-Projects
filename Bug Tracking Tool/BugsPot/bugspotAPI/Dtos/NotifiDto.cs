namespace bugspotAPI.Dtos
{
    public class NotifiDto
    {
        public string title { get; set; }
        public string message { get; set; }
        public string receiverId { get; set; }
        public bool viewed { get; set; }
    }
}