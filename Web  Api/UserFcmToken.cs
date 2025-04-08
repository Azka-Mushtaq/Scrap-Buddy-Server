using System.ComponentModel.DataAnnotations;

namespace Web__Api
{
    public class UserFcmToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FcmToken { get; set; }
        public bool IsActive { get; set; }

        public string Topic { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
