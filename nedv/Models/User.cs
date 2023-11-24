using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace nedv.Models
{
    public class User : IdentityUser
    {   
        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
       public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        public DateTime Birthday { get; set; }
    }
}
