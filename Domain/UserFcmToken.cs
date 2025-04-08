using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserFcmToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FcmToken { get; set; }

        public bool IsActive { get; set; }
        public string Topic { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
