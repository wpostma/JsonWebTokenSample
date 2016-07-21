using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JsonWebTokenSample.Models
{
    [Table("User_View")]
    public class Users
    {
        [Key]
        public long InternalUserID { get; set; }
        public long InternalGroupID { get; set; }
        public long InternalRoleID { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string ExtJson { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
