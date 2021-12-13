namespace bugspotAPI.Dtos
{
    public class RegisterDto
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}