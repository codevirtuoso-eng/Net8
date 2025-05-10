using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcWebApplication.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User password is required")]
        [Display(Name = "User Password")]
        public string UserPassword { get; set; }

        public bool IsAuthenticated { get; set; } = false;
    }
}
