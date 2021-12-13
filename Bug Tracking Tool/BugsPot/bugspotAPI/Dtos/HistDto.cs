namespace bugspotAPI.Dtos
{
    public class HistDto
    {
        public string userId { get; set; }
        public string changedItem { get; set; }
        public string oldValue { get; set; }
        public string newValue { get; set; }
    }
}