using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EkotaNibash.Domain
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        [Required]
        public Roles UserRole { get; set; }
        public bool IsDisable { get; set; }
    }

    public enum Roles
    {
        [Display(Name = "Administrator"),  Description("SA")]
        Administrator = 1,
        [Display(Name = "Admin"), Description("AD")]
        Admin = 2,
        [Display(Name = "Manager"), Description("MG")]
        Manager = 3,
        [Display(Name = "Sales"), Description("SM")]
        Sales = 4,
    }

    public enum UserGroupEnum
    {
        None = 0,
        SA = 1,
        AD = 2,
        MG = 3,
        SM = 4,
    }
}