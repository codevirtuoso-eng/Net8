using MvcWebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcWebApplication.ViewModels.Home
{
    public class HomeLoginViewModel : BaseViewModel
    {
        public UserLogin UserLogin { get; set; }
    }
}
