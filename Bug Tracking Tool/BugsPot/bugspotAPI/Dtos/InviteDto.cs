using System;

namespace bugspotAPI.Dtos
{
    public class InviteDto
    {
        public DateTimeOffset joinDate { get; set; }
        public int companyId { get; set; }
        public int projectId { get; set; }
        public string inviteeId { get; set; }
        public string inviteeEmail { get; set; }
        public string inviteeFName { get; set; }
        public string inviteeLName { get; set; }
        public bool IsValid { get; set; }
    }
}